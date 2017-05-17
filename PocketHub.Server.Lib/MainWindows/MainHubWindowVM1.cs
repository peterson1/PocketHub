using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using Repo2.Core.ns11.FileSystems;

namespace PocketHub.Server.Lib.MainWindows
{
    public class MainHubWindowVM1 : MainWindowBase
    {
        protected override string CaptionPrefix => "Hub";

        public MainHubWindowVM1(ActivityLogVM activityLogVM, ServerSettings serverSettings, ServerToggleVM serverToggleVM, ConnectionsTabVM connectionsTabVM, IFileSystemAccesor fs) : base(serverSettings, serverToggleVM, connectionsTabVM, fs)
        {
            AddAsTab(activityLogVM);
        }
    }
}
