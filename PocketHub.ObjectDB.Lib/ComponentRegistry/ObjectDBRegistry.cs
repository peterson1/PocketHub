using Autofac;
using Microsoft.Owin.Hosting;
using PocketHub.ObjectDB.Lib.MainWindows;
using PocketHub.ObjectDB.Lib.SignalRHubs;
using PocketHub.Server.Lib.ComponentRegistry;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using System;

namespace PocketHub.ObjectDB.Lib.ComponentRegistry
{
    public class ObjectDBRegistry : ServerRegistryBase
    {
        protected override void RegisterHubComponents(ContainerBuilder b)
        {
            b.Solo<IWebAppStarter, ObjectDBRegistry>();
            b.Solo<MainWindowVM1>();

            b.Multi<CompleteAccessHub1>();
        }


        protected override void RegisterHubs()
        {
            Register<CompleteAccessHub1>();
        }


        protected override object ResolveMainWindowVM()
            => Resolve<MainWindowVM1>();


        public override IDisposable StartWebApp(string hubServerUrl)
            => WebApp.Start<ObjectDBRegistry>(hubServerUrl);
    }
}
