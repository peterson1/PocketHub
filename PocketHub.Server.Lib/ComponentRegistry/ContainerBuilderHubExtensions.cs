using Autofac;
using Microsoft.AspNet.SignalR;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;

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
    }
}
