using Microsoft.AspNet.SignalR;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using PocketHub.Server.Lib.UserAccounts;
using Repo2.Core.ns11.ChangeNotification;
using System;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.SignalRHubs
{
    public abstract class SignalRHubBase : Hub, IStatusChanger
    {
        private      EventHandler<string> _statusChanged;
        public event EventHandler<string>  StatusChanged
        {
            add    { _statusChanged -= value; _statusChanged += value; }
            remove { _statusChanged -= value; }
        }


        private   ServerSettings    _cfg;
        protected ActivityLogVM     _log;
        private   IUserAccountsRepo _usrs;
        private   CurrentClientsVM  _clients;


        public SignalRHubBase(ActivityLogVM activityLogVM,
                              IUserAccountsRepo userAccountsRepo,
                              CurrentClientsVM currentClientsVM,
                              ServerSettings serverSettings)
        {
            _cfg      = serverSettings;
            _log      = activityLogVM;
            _usrs     = userAccountsRepo;
            _clients  = currentClientsVM;
        }


        public string Status { get; private set; }



        public override Task OnReconnected()
        {
            SetStatus("Reconnected");
            return base.OnReconnected();
        }


        public override async Task OnConnected()
        {
            SetStatus("Connected");
            await base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            SetStatus("Disconnected");
            return base.OnDisconnected(stopCalled);
        }


        protected void SetStatus(string message)
        {
            var msg = $"{CurrentUser?.ShortName} @{HubName} :  {message}";

            _clients.AddOrEdit(Context.ConnectionId, CurrentUser, HubName, message);
            
            _log.Info(msg);
            _statusChanged?.Raise(Status = msg);
        }


        protected string HubName => GetType().Name;


        protected UserAccount CurrentUser 
            => _usrs.FindAccount(Context.Request.GetUserName(_cfg));


        protected int? FindUserId(string loginName)
            => (int?)_usrs.FindAccount(loginName)?.Id;
    }
}
