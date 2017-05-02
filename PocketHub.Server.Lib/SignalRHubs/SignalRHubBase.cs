﻿using Microsoft.AspNet.SignalR;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
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
            var msg = $"{Current.Username} @{HubName} :  {message}";

            _clients.AddOrEdit(Context.ConnectionId, Current, HubName, message);
            
            _log.Info(msg);
            _statusChanged?.Raise(Status = msg);
        }


        protected string HubName => GetType().Name;

        protected IUserInfo Current 
            => _authSvr.GetProfile(Context.Request.GetUserName());
    }
}
