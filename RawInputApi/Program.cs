using RawInputApi.Presenters;
using RawInputApi.Views;
using System;
using System.Windows.Forms;

namespace RawInputApi
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ITestView view = new TestView();
            new MainPresenter(view);

            Application.Run(view as Form);
        }
    }
}
