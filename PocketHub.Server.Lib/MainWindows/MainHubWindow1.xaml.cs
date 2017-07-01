using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PocketHub.Server.Lib.MainWindows
{
    public partial class MainHubWindow1 : Window
    {
        const string DEFAULT_ICO_URI = "pack://application:,,,/PocketHub.Client.Lib;component/Assets/database128.ico";

        public MainHubWindow1(string trayIconURI = null)
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                var uri = new Uri(trayIconURI ?? DEFAULT_ICO_URI);
                trayIcon.IconSource = new BitmapImage(uri);
            };
            //trayIcon.ico
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


        //private MainHubWindowVM1 VM => DataContext as MainHubWindowVM1;
    }
}
