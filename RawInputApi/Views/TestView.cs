using RawInputApi.Presenters;
using System;
using System.Windows.Forms;

namespace RawInputApi.Views
{
    public partial class TestView : Form, ITestView
    {
        #region Constructor
        public TestView() => InitializeComponent();
        #endregion

        #region Interface implementation
        public IntPtr FormHandle => Handle;
        public MainPresenter Presenter { private get; set; }

        public void AddComboBoxItem(KeyPressEvent device)
        {
            comboBox.Items.Add(device);
        }

        public void Alert(string text)
        {
            MessageBox.Show(text);
        }

        public void AppendText(string vKeyName)
        {
            textBox.AppendText(vKeyName);
        }
        #endregion

        #region Event implementation
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Presenter.ActiveDeviceName = ((KeyPressEvent)comboBox.SelectedItem).DeviceName;
        }

        private void TestView_Shown(object sender, EventArgs e)
        {
            Presenter.ConnectRawInput();
        }

        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case Win32.WM_INPUT:
                    Presenter.ProcessRawInput(message.LParam);
                    break;
                case Win32.WM_USB_DEVICECHANGE:
                    Presenter.EnumerateDevices();
                    break;
            }

            base.WndProc(ref message);
        }
        #endregion
    }
}
