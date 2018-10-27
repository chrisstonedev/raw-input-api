using RawInputApi.Models;
using RawInputApi.Views;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RawInputApi.Presenters
{
    public class MainPresenter
    {
        #region Private fields
        private const int RI_KEY_BREAK = 1;

        private static readonly Guid deviceInterfaceHid = new Guid("4D1E55B2-F16F-11CF-88CB-001111000030");

        private static KeyboardDriver keyboardDriver;

        private readonly ITestView view;

        private IntPtr devNotifyHandle;
        private PreMessageFilter preMessageFilter;
        #endregion

        #region Constructor and destructor
        public MainPresenter(ITestView view)
        {
            this.view = view;
            this.view.Presenter = this;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        ~MainPresenter()
        {
            keyboardDriver.KeyPressed -= OnKeyPressed;
            Win32.UnregisterDeviceNotification(devNotifyHandle);

            // Remove message filter.
            if (preMessageFilter != null)
                Application.RemoveMessageFilter(preMessageFilter);
        }
        #endregion

        #region External methods
        public string ActiveDeviceName { get; set; }

        internal void ConnectRawInput()
        {
            IntPtr formHandle = view.FormHandle;

            keyboardDriver = new KeyboardDriver(formHandle);
            EnumerateDevices();

            // Register for device notifications.
            devNotifyHandle = IntPtr.Zero;
            BroadcastDeviceInterface bdi = new BroadcastDeviceInterface();
            bdi.Size = Marshal.SizeOf(bdi);
            bdi.BroadcastDeviceType = BroadcastDeviceType.DBT_DEVTYP_DEVICEINTERFACE;
            bdi.ClassGuid = deviceInterfaceHid;

            IntPtr mem = IntPtr.Zero;
            try
            {
                mem = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BroadcastDeviceInterface)));
                Marshal.StructureToPtr(bdi, mem, false);
                devNotifyHandle = Win32.RegisterDeviceNotification(formHandle, mem, DeviceNotification.DEVICE_NOTIFY_WINDOW_HANDLE);
            }
            catch (Exception e)
            {
                Debug.Print("Registration for device notifications Failed. Error: {0}", Marshal.GetLastWin32Error());
                Debug.Print(e.StackTrace);
            }
            finally
            {
                Marshal.FreeHGlobal(mem);
            }

            if (devNotifyHandle == IntPtr.Zero)
            {
                Debug.Print("Registration for device notifications Failed. Error: {0}", Marshal.GetLastWin32Error());
            }

            // Adding a message filter will cause keypresses to be handled.
            if (preMessageFilter == null)
            {
                preMessageFilter = new PreMessageFilter();
                Application.AddMessageFilter(preMessageFilter);
            }

            keyboardDriver.KeyPressed += OnKeyPressed;
            foreach (KeyPressEvent device in keyboardDriver.DeviceList.Values)
            {
                view.AddComboBoxItem(device);
            }
        }

        internal void EnumerateDevices()
        {
            Debug.WriteLine("USB Device Arrival / Removal");
            keyboardDriver.EnumerateDevices();
        }

        internal void ProcessRawInput(IntPtr lParam)
        {
            keyboardDriver.ProcessRawInput(lParam);
        }
        #endregion

        #region Event implementation
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!(e.ExceptionObject is Exception ex)) return;

            // Log this error. Logging the exception doesn't correct the problem but at least now
            // there is more insight as to why the exception is being thrown.
            Debug.WriteLine("Unhandled Exception: " + ex.Message);
            Debug.WriteLine("Unhandled Exception: " + ex);
            view.Alert(ex.Message);
        }

        private void OnKeyPressed(object sender, RawInputEventArgs e)
        {
            if (e.KeyPressEvent.IsKeyDown && e.KeyPressEvent.DeviceName == ActiveDeviceName)
            {
                view.AppendText(e.KeyPressEvent.VKeyName);
            }
        }
        #endregion
    }
}
