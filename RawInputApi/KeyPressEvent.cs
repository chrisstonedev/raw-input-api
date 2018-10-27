using System;

namespace RawInputApi
{
    public class KeyPressEvent
    {
        public string DeviceName; // i.e. \\?\HID#VID_045E&PID_00DD&MI_00#8&1eb402&0&0000#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}
        public IntPtr DeviceHandle; // Handle to the device that send the input
        public string Description; // i.e. Microsoft USB Comfort Curve Keyboard 2000 (Mouse and Keyboard Center)
        public bool IsKeyDown;
        public int VKey; // Virtual Key. Corrected for L/R keys(i.e. LSHIFT/RSHIFT) and Zoom
        public string VKeyName; // Virtual Key Name. Corrected for L/R keys(i.e. LSHIFT/RSHIFT) and Zoom

        public override string ToString()
        {
            return Description;
        }
    }
}


