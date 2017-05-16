using Autofac;
using PocketHub.ObjectDB.Lib.MainWindows;
using PocketHub.ObjectDB.Lib.SignalRHubs;
using PocketHub.Server.Lib.ComponentRegistry;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using System.Windows;

namespace PocketHub.ObjectDB.Lib.ComponentRegistry
{
    public class ObjectDBComponents : ComponentRegistryBase<ObjectDBHubs>
    {
        public ObjectDBComponents(Application app) : base(app)
        {
        }


        protected override void SetDataTemplates(Application app)
        {
        }

        protected override void RegisterHubComponents(ContainerBuilder b)
        {
            b.Solo<MainWindowVM1>();

            b.Multi<CompleteAccessHub1>();
        }


        public static T Resolve<T>()
            => ComponentRegistryBase.Resolve<T>();
    }
}
