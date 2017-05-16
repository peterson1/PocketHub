using PocketHub.ObjectDB.Lib.ComponentRegistry;
using PocketHub.Server.Lib.SignalRHubs;

namespace PocketHub.ObjectDB.Lib.SignalRHubs
{
    public class ObjectDBHubs : HubsRegistryBase
    {
        protected override void RegisterHubs()
        {
            Register<CompleteAccessHub1>();
        }

        protected override T ResolveHub<T>()
            => ObjectDBComponents.Resolve<T>();
    }
}
