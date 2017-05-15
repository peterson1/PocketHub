using Microsoft.AspNet.SignalR;
using Owin;
using PocketHub.Server.Lib.Configuration;

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


        private void SetGlobalHostConfigurations()
        {
            var cfg = ResolveHub<ServerSettings>();

            if (cfg.AllowBigContent)
                GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;
        }


        protected void Register<T>()
            => GlobalHost.DependencyResolver
                .Register(typeof(T), () => ResolveHub<T>());
    }
}
