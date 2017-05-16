using PocketHub.Client.Lib.UserInterfaces.Logging;
using PropertyChanged;
using Repo2.SDK.WPF45.ViewModelTools;

namespace PocketHub.Server.Lib.MainTabs.ConnectionsTab
{
    [ImplementPropertyChanged]
    public class ConnectionsTabVM : R2ViewModelBase
    {
        public ConnectionsTabVM(ServerToggleVM serverToggleVM,
                                ActivityLogVM activityLogVM,
                                CurrentClientsVM currentClientsVM)
        {
            UpdateTitle("Connections");

            ServerToggle   = serverToggleVM;
            CurrentClients = currentClientsVM;
            ActivityLog    = activityLogVM;
        }


        public ServerToggleVM     ServerToggle    { get; }
        public CurrentClientsVM   CurrentClients  { get; }
        public ActivityLogVM      ActivityLog     { get; }
    }
}
