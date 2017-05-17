using Autofac;
using PocketHub.Server.Lib.MainWindows;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using System;
using System.Windows;

namespace PocketHub.Server.Lib.ComponentRegistry
{
    public abstract class ServerRegistryBase2 : ServerRegistryBase
    {
        protected abstract void RegisterServerComponents(ContainerBuilder b);


        protected override void RegisterHubComponents(ContainerBuilder b)
        {
            b.Solo<MainHubWindowVM1>();

            RegisterServerComponents(b);
        }


        protected override void RegisterHubs()
        {
        }


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
