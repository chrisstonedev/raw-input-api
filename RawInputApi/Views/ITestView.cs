using RawInputApi.Presenters;
using System;

namespace RawInputApi.Views
{
    public interface ITestView
    {
        IntPtr FormHandle { get; }
        MainPresenter Presenter { set; }

        void AddComboBoxItem(KeyPressEvent device);
        void AppendText(string vKeyName);
        void Alert(string message);
    }
}
