using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using PocketHub.Server.Lib.SignalRHubs;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ViewModelTools;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.MainWindows
{
    public abstract class MainWindowBase : TabbedMainWindowBase
    {

        private ServerToggleVM _togl;
        private ServerSettings _cfg;


        public MainWindowBase(ServerSettings serverSettings,
                              ServerToggleVM serverToggleVM,
                              ConnectionsTabVM connectionsTabVM,
                              IFileSystemAccesor fs) : base(fs)
        {
            _cfg = serverSettings;
            _togl = serverToggleVM;

            _togl.ServerStarted += (s, e) => AppendToCaption("Accepting Connections");
            _togl.ServerStopped += (s, e) => AppendToCaption("Connections NOT accepted");

            AddAsTab(connectionsTabVM);

            _togl.StartServerCmd.ExecuteIfItCan();
        }


        protected override async Task BeforeExitApp()
        {
            StartBeingBusy("Closing all connections...");
            _togl.StopServerCmd.ExecuteIfItCan();
            await Task.Delay(500);
            StopBeingBusy();
        }


        protected override void AppendToCaption(string text)
            => Caption = $"{CaptionPrefix}  v.{_exeVer}  :  {_cfg?.HubServerURL}  :  {text}";
    }
}
