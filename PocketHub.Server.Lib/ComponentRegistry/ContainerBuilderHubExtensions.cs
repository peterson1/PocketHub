using Autofac;
using Microsoft.AspNet.SignalR;
using PocketHub.Server.Lib.MainWindows;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.Extensions.ViewModelExtensions;
using Repo2.SDK.WPF45.ViewModelTools;
using System.Windows;

namespace PocketHub.Server.Lib.ComponentRegistry
{
    public static class ContainerBuilderHubExtensions
    {
        public static void Hub <T>(this ContainerBuilder b)
        {
            b.Multi<T>();
            GlobalHost.DependencyResolver
                .Register(typeof(T), () => StaticRegistry.Resolve<T>());
        }


        public static void Tab<TVM, TUI>(this ContainerBuilder b, Application app)
            where TVM : R2ViewModelBase
        {
            b.Solo<TVM>();
            app.SetTemplate<TVM, TUI>();
            MainHubWindowVM1.TabTypes.Add(typeof(TVM));
        }
    }
}
