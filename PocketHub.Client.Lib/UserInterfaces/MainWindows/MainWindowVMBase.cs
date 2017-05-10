using PocketHub.Client.Lib.Configuration;
using PocketHub.Client.Lib.ServiceContracts;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.InputCommands;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.SDK.WPF45.ViewModelTools;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.UserInterfaces.MainWindows
{
    public abstract class MainWindowVMBase : TabbedMainWindowBase
    {
        protected ClientSettings _cfg;
        protected IHubClient     _client;

        public MainWindowVMBase(IHubClient hubClient,
                                ActivityLogVM activityLogVM,
                                ClientSettings clientSettings,
                                IFileSystemAccesor fs) : base(fs)
        {
            _cfg          = clientSettings;
            _client       = hubClient;
            ActivityLog   = activityLogVM;
            ConnectCmd    = R2Command.Async(ConnectClient, _ => !_client.IsConnected, "Connect");
            DisconnectCmd = R2Command.Relay(_client.Disconnect, _ => _client.IsConnected, "Disconnect");

            _client.StatusChanged       += (s, e) => ActivityLog.Info(e);
            _client.ConnectStateChanged += (s, e) => AppendToCaption(e.ToString());

            ConnectCmd.ExecuteIfItCan();
        }


        public IR2Command      ConnectCmd     { get; }
        public IR2Command      DisconnectCmd  { get; }
        public ActivityLogVM   ActivityLog    { get; }


        public abstract string GetLoginName();
        public abstract string GetRawPassword();


        private async Task ConnectClient()
        {
            var usr = GetLoginName();

            StartBeingBusy($"Connecting to [{_cfg.HubServerURL}] as “{usr}” ...");
            var hubReply = await _client.Connect(_cfg.HubServerURL, usr, GetRawPassword(), _cfg.SharedKey);
            if (hubReply.Failed) Alerter.Show(hubReply, "Reply from Hub Server");
            StopBeingBusy();
        }


        protected override async Task BeforeExitApp()
        {
            StartBeingBusy("Closing all connections...");
            DisconnectCmd.ExecuteIfItCan();
            await Task.Delay(500);
            StopBeingBusy();
        }


        protected override void AppendToCaption(string text)
            => Caption = $"{CaptionPrefix}  v.{_exeVer}  :  {_cfg?.HubServerURL}  :  {text}";
    }
}
