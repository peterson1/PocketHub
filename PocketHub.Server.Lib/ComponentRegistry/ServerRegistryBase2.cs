using Autofac;
using PocketHub.Server.Lib.MainWindows;
using PocketHub.Server.Lib.SignalRHubs;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using System;
using System.Windows;

namespace PocketHub.Server.Lib.ComponentRegistry
{
    public abstract class ServerRegistryBase2 : ServerRegistryBase
    {
        protected abstract void RegisterServerComponents(ContainerBuilder b, Application app);


        protected override void RegisterHubComponents(ContainerBuilder b, Application app)
        {
            MainHubWindowVM1.Title = MainWindowTitle;

            b.Solo<MainHubWindowVM1>();
            b.Hub<UserAccountHub>();

            RegisterServerComponents(b, app);
        }


        protected override void RegisterHubs()
        {
        }


        protected virtual string MainWindowTitle => "Hub Server";


        protected override object ResolveMainWindowVM()
            => Resolve<MainHubWindowVM1>();


        protected override void AddResourceDictionaries(Application app)
            => AddToMergedDictionaries(app,
                @"/Repo2.SDK.WPF45;component/Styles/BasicDatagridTheme1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/DatagridTheme2.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/FontAwesomeButtonTheme1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/ConvertersSet1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/NonReloadingTabControlTheme1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/ButtonsTheme1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/TextBlockTheme1.xaml");


        private void AddToMergedDictionaries(Application app, params string[] sources)
        {
            foreach (var src in sources)
            {
                var uri = new Uri(src, UriKind.Relative);
                var res = new ResourceDictionary { Source = uri };
                app.Resources.MergedDictionaries.Add(res);
            }
        }
    }
}
