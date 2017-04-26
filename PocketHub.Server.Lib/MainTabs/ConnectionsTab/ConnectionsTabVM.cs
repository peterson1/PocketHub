using PocketHub.Server.Lib.Logging;
using PocketHub.Server.Lib.SignalRHubs;
using PropertyChanged;
using Repo2.SDK.WPF45.ViewModelTools;

namespace PocketHub.Server.Lib.MainTabs.ConnectionsTab
{
    [ImplementPropertyChanged]
    public class ConnectionsTabVM<T> : R2ViewModelBase
        where T : HubsRegistryBase
    {
        public ConnectionsTabVM(ServerToggleVM<T> serverToggleVM,
                                ActivityLogVM activityLogVM,
                                CurrentClientsVM currentClientsVM)
        {
            UpdateTitle("Connections");

            ServerToggle   = serverToggleVM;
            ActivityLog    = activityLogVM;
            CurrentClients = currentClientsVM;
        }


        public ServerToggleVM<T>  ServerToggle    { get; }
        public ActivityLogVM      ActivityLog     { get; }
        public CurrentClientsVM   CurrentClients  { get; }
    }
}
