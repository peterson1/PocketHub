using Autofac;
using Microsoft.AspNet.SignalR;
using Owin;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authorization;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using PocketHub.Server.Lib.UserAccounts;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.Extensions.ViewModelExtensions;
using System;
using System.Windows;

namespace PocketHub.Server.Lib.ComponentRegistry
{
    public abstract class ServerRegistryBase : IWebAppStarter
    {
        public    abstract IDisposable  StartWebApp            (string hubServerUrl);
        protected abstract void         RegisterHubs           ();
        protected abstract object       ResolveMainWindowVM    ();
        protected abstract void         RegisterHubComponents  (ContainerBuilder b, Application app);


        public object CreateMainVM (Application app)
        {
            if (app != null)
            {
                app.SetTemplate<ConnectionsTabVM, ConnectionsTabUI>();
                app.SetTemplate<ActivityLogVM, ActivityLogUI>();
                AddResourceDictionaries(app);
                SetDataTemplates(app);
            }

            var containr = RegisterAllComponents(app);
            StaticRegistry.BeginLifetimeScope(containr);
            return ResolveMainWindowVM();
        }

        public void Configuration(IAppBuilder appBuildr)
        {
            RegisterHubs();

            appBuildr.MapSignalR(new HubConfiguration
            {
                //todo: disable on production
                EnableDetailedErrors = true
            });

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



        public T Resolve<T>() => StaticRegistry.Resolve<T>();


        protected virtual void SetDataTemplates(Application app)
        {
        }


        protected virtual void AddResourceDictionaries(Application app)
        {
        }


        private IContainer RegisterAllComponents(Application app)
        {
            var b = new ContainerBuilder();
            Repo2IoC.RegisterComponentsTo(ref b);

            b.Solo<IUserAccountsRepo, UserAccountsLocalRepo1>();
            b.Solo<IUserAuthChecker, UserAuthChecker1>();

            b.Solo<ConnectionsTabVM>();
            b.Solo<ServerToggleVM>();

            b.Solo<CurrentClientsVM>();
            b.Solo<ActivityLogVM>();

            RegisterHubComponents(b, app);

            var cfg = ServerSettings.LoadFile();
            b.RegisterInstance(cfg).AsSelf();
            return b.Build();
        }
    }
}
