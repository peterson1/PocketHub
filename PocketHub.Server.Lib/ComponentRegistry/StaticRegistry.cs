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

namespace PocketHub.Server.Lib.ComponentRegistry
{
    public static class StaticRegistry
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
