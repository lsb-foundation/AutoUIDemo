using System;
using System.Windows;

namespace AutoUIDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += (s, se) =>
            {
                MessageBox.Show("Exception:" + se.Exception.Message + Environment.NewLine + se.Exception.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
                se.Handled = true;
            };
        }
    }
}
