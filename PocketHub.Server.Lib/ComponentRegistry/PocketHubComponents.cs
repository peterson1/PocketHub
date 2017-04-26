using Autofac;
using PocketHub.Server.Lib.Logging;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;

namespace PocketHub.Server.Lib.ComponentRegistry
{
    public class PocketHubComponents
    {
        public static void RegisterTo(ref ContainerBuilder b)
        {
            //b.Solo<ConnectionsTabVM>();
            //b.Solo<ServerToggleVM>();

            b.RegisterGeneric(typeof(ConnectionsTabVM<>)).AsSelf().SingleInstance();
            b.RegisterGeneric(typeof(ServerToggleVM<>)).AsSelf().SingleInstance();

            b.Solo<CurrentClientsVM>();
            b.Solo<ActivityLogVM>();
        }
    }
}
