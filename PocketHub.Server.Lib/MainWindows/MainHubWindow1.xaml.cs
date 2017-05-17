using System.ComponentModel;
using System.Windows;

namespace PocketHub.Server.Lib.MainWindows
{
    public partial class MainHubWindow1 : Window
    {
        public MainHubWindow1()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void TaskbarIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.Activate();
        }
    }
}
