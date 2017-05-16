using PocketHub.ObjectDB.Lib.ComponentRegistry;
using PocketHub.ObjectDB.Lib.MainWindows;
using System.Windows;

namespace PocketHub.ObjectDB.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            new ObjectDBComponents(this);
            var win         = new MainWindow1();
            win.DataContext = ObjectDBComponents.Resolve<MainWindowVM1>();
            win.Show();
        }
    }
}
