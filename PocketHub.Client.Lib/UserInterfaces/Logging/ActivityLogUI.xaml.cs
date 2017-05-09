using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PocketHub.Client.Lib.UserInterfaces.Logging
{
    public partial class ActivityLogUI : UserControl
    {
        public ActivityLogUI()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                lboxRows.MouseDoubleClick += LboxRows_MouseDoubleClick;
            };
        }

        private void LboxRows_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(lboxRows.SelectedItem.ToString());
        }
    }
}
