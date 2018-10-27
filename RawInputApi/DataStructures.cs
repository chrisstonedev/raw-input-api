using System;
using System.Runtime.InteropServices;

namespace RawInputApi
{
    internal struct BroadcastDeviceInterface
    {
        public int Size;
        public BroadcastDeviceType BroadcastDeviceType;
        public Guid ClassGuid;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RawInputDeviceList
    {
        public IntPtr Device;
        public uint Type;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct RawData
    {
        [FieldOffset(0)]
        internal RawKeyboardStruct Keyboard;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct InputData
    {
        public RawInputHeader Header; // 64 bit header size: 24  32 bit the header size: 16
        public RawData Data; // Creating the rest in a struct allows the header size to align correctly for 32/64 bit
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputHeader
    {
        public uint Type; // Type of raw input (RIM_TYPEHID 2, RIM_TYPEKEYBOARD 1, RIM_TYPEMOUSE 0)
        public uint Size; // Size in bytes of the entire input packet of data. This includes RAWINPUT plus possible extra input reports in the RAWHID variable length array. 
        public IntPtr Device; // A handle to the device generating the raw input data. 
        public IntPtr Parameters; // RIM_INPUT 0 if input occurred while application was in the foreground else RIM_INPUTSINK 1 if it was not.
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RawKeyboardStruct
    {
        public ushort Makecode; // Scan code from the key depression
        public ushort Flags; // One or more of RI_KEY_MAKE, RI_KEY_BREAK, RI_KEY_E0, RI_KEY_E1
        private readonly ushort Reserved; // Always 0    
        public ushort VKey; // Virtual Key Code
        public uint Message; // Corresponding Windows message for exmaple (WM_KEYDOWN, WM_SYASKEYDOWN etc)
        public uint ExtraInformation; // The device-specific addition information for the event (seems to always be zero for keyboards)
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RawInputDevice
    {
        internal HidUsagePage UsagePage;
        internal HidUsage Usage;
        internal RawInputDeviceFlags Flags;
        internal IntPtr Target;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeMessage
    {
        public IntPtr handle;
        public uint msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }
}
