using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace RawInputApi.Models
{
    public sealed class KeyboardDriver
    {
        public Dictionary<IntPtr, KeyPressEvent> DeviceList { get; private set; } = new Dictionary<IntPtr, KeyPressEvent>();

        public delegate void DeviceEventHandler(object sender, RawInputEventArgs e);
        public event DeviceEventHandler KeyPressed;

        private readonly object lockObject = new object();

        private static InputData rawBuffer;

        public KeyboardDriver(IntPtr hwnd)
        {
            RawInputDevice[] rid = new RawInputDevice[] { new RawInputDevice
            {
                UsagePage = HidUsagePage.Generic,
                Usage = HidUsage.Keyboard,
                Flags = RawInputDeviceFlags.InputSink | RawInputDeviceFlags.DevNotify,
                Target = hwnd
            }};

            if (!Win32.RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])))
            {
                throw new ApplicationException("Failed to register raw input device(s).");
            }
        }

        public void EnumerateDevices()
        {
            lock (lockObject)
            {
                DeviceList.Clear();

                uint deviceCount = 0;
                int dwSize = Marshal.SizeOf(typeof(RawInputDeviceList));

                if (Win32.GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)dwSize) == 0)
                {
                    IntPtr pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
                    Win32.GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint)dwSize);

                    for (int i = 0; i < deviceCount; i++)
                    {
                        uint pcbSize = 0;

                        // On Window 8 64bit when compiling against .Net > 3.5 using .ToInt32 you will generate an arithmetic overflow. Leave as it is for 32bit/64bit applications
                        RawInputDeviceList rid = (RawInputDeviceList)Marshal.PtrToStructure(new IntPtr(pRawInputDeviceList.ToInt64() + (dwSize * i)), typeof(RawInputDeviceList));

                        Win32.GetRawInputDeviceInfo(rid.Device, RawInputDeviceInfo.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);

                        if (pcbSize <= 0) continue;

                        IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
                        Win32.GetRawInputDeviceInfo(rid.Device, RawInputDeviceInfo.RIDI_DEVICENAME, pData, ref pcbSize);
                        string deviceName = Marshal.PtrToStringAnsi(pData);

                        if (rid.Type == (uint)DeviceType.Keyboard && deviceName.Length > 0 && deviceName.Count(x => x == '#') >= 2)
                        {
                            try
                            {
                                string[] split = deviceName.Substring(4).Split('#');

                                string classCode = split[0]; // ACPI (Class code)
                                string subClassCode = split[1]; // PNP0303 (SubClass code)
                                string protocolCode = split[2]; // 3&13c0b0c5&0 (Protocol code)

                                RegistryKey deviceKey = Registry.LocalMachine.OpenSubKey($@"System\CurrentControlSet\Enum\{classCode}\{subClassCode}\{protocolCode}");

                                string deviceDesc = deviceKey.GetValue("DeviceDesc").ToString();
                                deviceDesc = deviceDesc.Substring(deviceDesc.IndexOf(';') + 1);

                                KeyPressEvent deviceInfo = new KeyPressEvent
                                {
                                    DeviceName = Marshal.PtrToStringAnsi(pData),
                                    DeviceHandle = rid.Device,
                                    Description = deviceDesc,
                                };

                                if (!DeviceList.ContainsKey(rid.Device))
                                {
                                    DeviceList.Add(rid.Device, deviceInfo);
                                }
                            }
                            catch (Exception) { } // Ignore error and try next device.
                        }

                        Marshal.FreeHGlobal(pData);
                    }

                    Marshal.FreeHGlobal(pRawInputDeviceList);
                    return;
                }
            }

            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public void ProcessRawInput(IntPtr hdevice)
        {
            if (DeviceList.Count == 0)
                return;

            int dwSize = 0;
            Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, IntPtr.Zero, ref dwSize, Marshal.SizeOf(typeof(RawInputHeader)));

            if (dwSize != Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, out rawBuffer, ref dwSize, Marshal.SizeOf(typeof(RawInputHeader))))
            {
                Debug.WriteLine("Error getting the rawinput buffer");
                return;
            }

            int virtualKey = rawBuffer.Data.Keyboard.VKey;
            int makeCode = rawBuffer.Data.Keyboard.Makecode;
            int flags = rawBuffer.Data.Keyboard.Flags;

            if (virtualKey == Win32.KEYBOARD_OVERRUN_MAKE_CODE)
                return;

            bool isE0BitSet = (flags & Win32.RI_KEY_E0) != 0;

            KeyPressEvent keyPressEvent;

            if (DeviceList.ContainsKey(rawBuffer.Header.Device))
            {
                lock (lockObject)
                {
                    keyPressEvent = DeviceList[rawBuffer.Header.Device];
                }
            }
            else
            {
                Debug.WriteLine("Handle: {0} was not in the device list.", rawBuffer.Header.Device);
                return;
            }

            keyPressEvent.IsKeyDown = rawBuffer.Data.Keyboard.Message == Win32.WM_KEYDOWN;
            keyPressEvent.VKeyName = KeyMapper.GetKeyName(VirtualKeyCorrection(virtualKey, isE0BitSet, makeCode)).ToUpper();
            keyPressEvent.VKey = virtualKey;

            KeyPressed?.Invoke(this, new RawInputEventArgs(keyPressEvent));
        }

        private static int VirtualKeyCorrection(int virtualKey, bool isE0BitSet, int makeCode)
        {
            if (rawBuffer.Header.Device == IntPtr.Zero)
            {
                // When hDevice is 0 and the vkey is VK_CONTROL indicates the ZOOM key
                if (rawBuffer.Data.Keyboard.VKey == Win32.VK_CONTROL)
                    return Win32.VK_ZOOM;
                else
                    return virtualKey;
            }
            else
            {
                switch (virtualKey)
                {
                    // Right-hand CTRL and ALT have their e0 bit set 
                    case Win32.VK_CONTROL:
                        return isE0BitSet ? Win32.VK_RCONTROL : Win32.VK_LCONTROL;
                    case Win32.VK_MENU:
                        return isE0BitSet ? Win32.VK_RMENU : Win32.VK_LMENU;
                    case Win32.VK_SHIFT:
                        return makeCode == Win32.SC_SHIFT_R ? Win32.VK_RSHIFT : Win32.VK_LSHIFT;
                    default:
                        return virtualKey;
                }
            }
        }
    }
}
