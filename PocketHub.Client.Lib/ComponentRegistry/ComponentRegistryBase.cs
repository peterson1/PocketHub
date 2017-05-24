using Autofac;
using Autofac.Builder;
using Autofac.Core;
using PocketHub.Client.Lib.Configuration;
using PocketHub.Client.Lib.ServiceProviders;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.Extensions.ViewModelExtensions;
using System;
using System.Windows;

namespace PocketHub.Client.Lib.ComponentRegistry
{
    public abstract class ComponentRegistryBase : IDisposable
    {
        private ILifetimeScope   _scope;
        private ContainerBuilder _buildr;

        public ComponentRegistryBase(Application app, bool beginScope = true)
        {
            app.SetTemplate<ActivityLogVM, ActivityLogUI>();
            SetDataTemplates(app);

            _buildr = CreateContainerBuilder();
            if (beginScope) BeginLifetimeScope();
        }

        protected abstract void  SetDataTemplates          (Application app);
        protected abstract void  RegisterClientComponents  (ContainerBuilder b);


        public void BeginLifetimeScope()
            => _scope = _buildr.Build().BeginLifetimeScope();


        public IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterSolo<T>()
            => _buildr.RegisterType<T>().AsSelf().SingleInstance();


        public T Resolve<T>()
        {
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


        private ContainerBuilder CreateContainerBuilder()
        {
            var b = new ContainerBuilder();
            Repo2IoC.RegisterComponentsTo(ref b);

            b.Solo<ActivityLogVM>();
            b.Solo<UserAccountClient>();

            RegisterClientComponents(b);

            var cfg = ClientSettings.LoadFile();
            b.RegisterInstance(cfg).AsSelf();
            return b;
        }



        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                _scope?.Dispose();
                _scope = null;
            }
            _disposedValue = true;
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
