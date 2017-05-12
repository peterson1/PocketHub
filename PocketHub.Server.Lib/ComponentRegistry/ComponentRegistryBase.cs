using Autofac;
using Autofac.Core;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.Authorization;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using PocketHub.Server.Lib.SignalRHubs;
using PocketHub.Server.Lib.UserAccounts;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Exceptions;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.Extensions.ViewModelExtensions;
using System.Windows;

namespace PocketHub.Server.Lib.ComponentRegistry
{
    public abstract class ComponentRegistryBase<T>
        where T : HubsRegistryBase
    {
        public ComponentRegistryBase(Application app)
        {
            app.SetTemplate<ConnectionsTabVM<T>, ConnectionsTabUI>();

            SetDataTemplates(app);

            var containr = RegisterAllComponents();
            ComponentRegistryBase.BeginLifetimeScope(containr);
        }

        protected abstract void SetDataTemplates      (Application app);
        protected abstract void RegisterHubComponents (ContainerBuilder b);


        private IContainer RegisterAllComponents()
        {
            var b = new ContainerBuilder();
            Repo2IoC.RegisterComponentsTo(ref b);

            b.Solo<IUserAccountsRepo, UserAccountsLocalRepo1>();
            b.Solo<IUserAuthChecker, UserAuthChecker1>();

            b.Solo<ConnectionsTabVM<T>>();
            b.Solo<ServerToggleVM<T>>();

            b.Solo<CurrentClientsVM>();
            b.Solo<ActivityLogVM>();

            RegisterHubComponents(b);

            var cfg = ServerSettings.LoadFile();
            b.RegisterInstance(cfg).AsSelf();
            return b.Build();
        }


    }

    public static class ComponentRegistryBase
    {
        private static ILifetimeScope _scope;


        public static void BeginLifetimeScope(IContainer containr)
        {
            _scope = containr.BeginLifetimeScope();
        }


        public static T Resolve<T>()
        {
            if (_scope == null)
                throw Fault.BadCall(nameof(BeginLifetimeScope));

            try
            {
                return _scope.Resolve<T>();
            }
            catch (DependencyResolutionException ex)
            {
                Alerter.ShowError("Resolver Error", ex.GetMessage());
                return default(T);
            }
        }
    }
}
