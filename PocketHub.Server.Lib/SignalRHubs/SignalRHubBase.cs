using Microsoft.AspNet.SignalR;
using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.Logging;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.SignalRHubs
{
    public abstract class SignalRHubBase : Hub
    {
        protected ActivityLogVM          _log;
        private   CurrentClientsVM       _clients;
        private   AuthServerTokenChecker _authSvr;


        public SignalRHubBase(ActivityLogVM activityLogVM,
                              AuthServerTokenChecker authServerTokenChecker,
                              CurrentClientsVM currentClientsVM)
        {
            _log     = activityLogVM;
            _clients = currentClientsVM;
            _authSvr = authServerTokenChecker;
        }


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


        private void SetStatus(string connectionState)
        {
            var connId = Context.ConnectionId;
            var hubNme = GetType().Name;

            _clients.AddOrEdit(connId, Current, hubNme, connectionState);
            
            _log.Info($"“{connId}” @‹{hubNme}› :  {connectionState}");
        }


        protected IUserInfo Current 
            => _authSvr.GetProfile(Context.Request.GetUserName());
    }
}
