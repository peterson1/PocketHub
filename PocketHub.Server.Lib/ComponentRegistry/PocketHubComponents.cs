using Autofac;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.Logging;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using Repo2.Core.ns11.Authentication;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.Extensions.ViewModelExtensions;
using System.Windows;

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

            var cfg = ServerSettings.LoadFile();
            b.RegisterInstance(cfg).As<IR2Credentials>()
                                   .AsSelf();
        }


        public static void SetCommonTemplates(Application app)
        {
            app.SetTemplate<ConnectionsTabVM<>, ConnectionsTabUI>();
        }
    }
}
