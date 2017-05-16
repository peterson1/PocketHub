using PocketHub.ObjectDB.Lib.SignalRHubs;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using PocketHub.Server.Lib.MainWindows;
using Repo2.Core.ns11.FileSystems;

namespace PocketHub.ObjectDB.Lib.MainWindows
{
    public class MainWindowVM1 : MainWindowBase
    {
        public MainWindowVM1(ServerSettings serverSettings, ServerToggleVM serverToggleVM, ConnectionsTabVM connectionsTabVM, IFileSystemAccesor fs) : base(serverSettings, serverToggleVM, connectionsTabVM, fs)
        {
        }

        protected override string CaptionPrefix => "Object DB Hub";

    }
}
