using PocketHub.Client.Lib.Authentication;
using PocketHub.Client.Lib.Configuration;
using PocketHub.Client.Lib.ServiceContracts;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.InputCommands;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.SDK.WPF45.ViewModelTools;
using System;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.UserInterfaces.MainWindows
{
    public abstract class MainWindowVMBase<T> : TabbedMainWindowBase
        where T : IHubClient
    {
        private   AuthServerTokenSetter _authSetr;
        protected ClientSettings        _cfg;
        protected T                     _client;

        public MainWindowVMBase(T hubClient,
                                ActivityLogVM activityLogVM,
                                ClientSettings clientSettings,
                                AuthServerTokenSetter authServerTokenSetter,
                                IFileSystemAccesor fs) : base(fs)
        {
            _cfg          = clientSettings;
            _client       = hubClient;
            _authSetr     = authServerTokenSetter;
            ActivityLog   = activityLogVM;
            ConnectCmd    = R2Command.Async(ConnectClient, _ => !_client.IsConnected, "Connect");
            DisconnectCmd = R2Command.Relay(_client.Disconnect, _ => _client.IsConnected, "Disconnect");

            _client.StatusChanged += (s, e) =>
            {
                AppendToCaption(e);
                ActivityLog.Info(e);
            };
            ConnectCmd.ExecuteIfItCan();
        }


        public IR2Command      ConnectCmd     { get; }
        public IR2Command      DisconnectCmd  { get; }
        public ActivityLogVM   ActivityLog    { get; }



        private async Task ConnectClient()
        {
            StartBeingBusy($"Authenticating as “{_cfg.AuthServerUsername}” ...");
            var tknReply = await _authSetr.PostNewToken();
            if (tknReply.Failed)
            {
                Alerter.Show(tknReply, "Reply from Authentication Server");
                StopBeingBusy();
                return;
            }

            StartBeingBusy($"Connecting to [{_cfg.HubServerURL}] ...");
            var hubReply = await _client.Connect(_cfg.HubServerURL, _cfg.AuthServerUsername, tknReply.Result);
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
