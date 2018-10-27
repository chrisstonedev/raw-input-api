using System;

namespace RawInputApi
{
    public class RawInputEventArgs : EventArgs
    {
        public RawInputEventArgs(KeyPressEvent arg)
        {
            KeyPressEvent = arg;
        }

        public KeyPressEvent KeyPressEvent { get; private set; }
    }
}
