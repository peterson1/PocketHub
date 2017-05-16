using Autofac;
using Autofac.Core;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authorization;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using PocketHub.Server.Lib.SignalRHubs;
using PocketHub.Server.Lib.UserAccounts;
using Repo2.Core.ns11.Exceptions;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.Extensions.ViewModelExtensions;
using System.Windows;
using System;
using Owin;
using Microsoft.AspNet.SignalR;

namespace PocketHub.Server.Lib.ComponentRegistry
{
    public abstract class ServerRegistryBase : IWebAppStarter
    {
        public    abstract IDisposable  StartWebApp            (string hubServerUrl);
        protected abstract void         RegisterHubs           ();
        protected abstract object       ResolveMainWindowVM    ();
        protected abstract void         RegisterHubComponents  (ContainerBuilder b);


        public object CreateMainVM (Application app)
        {
            if (app != null)
            {
                app.SetTemplate<ConnectionsTabVM, ConnectionsTabUI>();
                SetDataTemplates(app);
            }

            var containr = RegisterAllComponents();
            ComponentRegistryBase.BeginLifetimeScope(containr);
            return ResolveMainWindowVM();
        }


        public void Configuration(IAppBuilder app)
        {
            RegisterHubs();

            app.MapSignalR();

            SetGlobalHostConfigurations();
        }


        private void SetGlobalHostConfigurations()
        {
            var cfg = Resolve<ServerSettings>();

            if (cfg.AllowBigContent)
                GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;
        }


        protected void Register<T>()
            => GlobalHost.DependencyResolver
                .Register(typeof(T), () => Resolve<T>());



        protected T Resolve<T>() => ComponentRegistryBase.Resolve<T>();


        protected virtual void SetDataTemplates(Application app)
        {
        }


        private IContainer RegisterAllComponents()
        {
            var b = new ContainerBuilder();
            Repo2IoC.RegisterComponentsTo(ref b);

            b.Solo<IUserAccountsRepo, UserAccountsLocalRepo1>();
            b.Solo<IUserAuthChecker, UserAuthChecker1>();

            b.Solo<ConnectionsTabVM>();
            b.Solo<ServerToggleVM>();

            b.Solo<CurrentClientsVM>();
            b.Solo<ActivityLogVM>();

            RegisterHubComponents(b);

            var cfg = ServerSettings.LoadFile();
            b.RegisterInstance(cfg).AsSelf();
            return b.Build();
        }
    }
}
