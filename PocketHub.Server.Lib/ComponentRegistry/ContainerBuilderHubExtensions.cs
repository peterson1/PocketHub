using Autofac;
using Microsoft.AspNet.SignalR;
using PocketHub.Server.Lib.MainWindows;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.ViewModelTools;

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


        public static void Tab<T>(this ContainerBuilder b)
            where T : R2ViewModelBase
        {
            b.Solo<T>();
            MainHubWindowVM1.TabTypes.Add(typeof(T));
        }
    }
}
