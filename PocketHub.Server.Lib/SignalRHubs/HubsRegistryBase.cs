using Microsoft.AspNet.SignalR;
using Owin;

namespace PocketHub.Server.Lib.SignalRHubs
{
    public abstract class HubsRegistryBase
    {
        protected abstract void RegisterHubs();
        protected abstract T    ResolveHub<T>();


        public void Configuration(IAppBuilder app)
        {
            RegisterHubs();

            app.MapSignalR();

            SetGlobalHostConfigurations();
        }


        protected virtual void SetGlobalHostConfigurations()
        {
        }


        protected void Register<T>()
            => GlobalHost.DependencyResolver
                .Register(typeof(T), () => ResolveHub<T>());
    }
}
