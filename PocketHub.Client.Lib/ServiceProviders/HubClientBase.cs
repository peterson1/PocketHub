using Microsoft.AspNet.SignalR.Client;
using PocketHub.Client.Lib.ServiceContracts;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceProviders
{
    public abstract class HubClientBase : StatusChanger, IHubClient
    {
        private      EventHandler<ConnectionStatus> _connectStateChanged;
        public event EventHandler<ConnectionStatus>  ConnectStateChanged
        {
            add    { _connectStateChanged -= value; _connectStateChanged += value; }
            remove { _connectStateChanged -= value; }
        }

        protected IHubProxy              _hub;
        private   HubConnection          _conn;
        private   SynchronizationContext _ui;


        public HubClientBase()
        {
            _ui = SynchronizationContext.Current;
        }


        //public void UseConnectionFrom(HubClientBase client)
        //{
        //    _conn = client._conn;
        //    _hub  = client._hub;
        //}


        protected abstract string HubName { get; }

        public bool              IsConnected   => _conn?.State == ConnectionState.Connected;
        public ConnectionStatus  ConnectState  => GetConnectionState();



        public async Task<Reply> Connect(string url, string usr, string authTokn)
        {
            Disconnect();

            SetStatus($"Connecting to [{url}] as “{usr}” ...");
            _conn = new HubConnection(url);

            _conn.Headers.Add("username" , usr);
            _conn.Headers.Add("auth-token", authTokn);

            HandleConnectionEvents();

            _hub = _conn.CreateHubProxy(HubName);
            HandleHubEvents();

            try
            {
                await _conn.Start();
            }
            catch (HttpRequestException ex)
            {
                return Reply.Error($"Server might not be running.{L.f}{ex.Info()}");
            }
            catch (HttpClientException ex)
            {
                return Reply.Error($"[{(int)ex.Response.StatusCode}] {ex.Response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                return Reply.Error(ex.Info(true, true));
            }

            if (IsConnected)
            {
                SetStatus("Successfully connected to server.");
                OnConnected();
                return Reply.Success();
            }
            else
            {
                SetStatus("Failed to connect to server.");
                return Reply.Error("Failed to connect.");
            }
        }


        protected async Task<Reply<T>> Invoke<T>(string methodName, params object[] args)
        {
            SetStatus($"Invoking ‹{HubName}›.{methodName}() ...");
            try
            {
                var rep = await _hub.Invoke<Reply<T>>(methodName, args);
                SetStatus(rep.IsSuccessful ? $"Successfully returned: [{rep.Result}]" : rep.ErrorsText);
                return rep;
            }
            catch (Exception ex)
            {
                return Reply.Error<T>(ex.Info());
            }
        }


        protected virtual void OnConnected()
        {
        }


        private void HandleHubEvents()
        {
            _hub.On<string, string>("AddMessage", (nme, msg) 
                => SetStatus($"[{nme}]  {msg}"));
        }


        private void HandleConnectionEvents()
        {
            if (_conn == null) return;
            _conn.Closed         += () => SetStatus("Connection closed.");
            _conn.ConnectionSlow += () => SetStatus("Slow connection is about to timeout ...");
            _conn.Error          += ex => SetStatus(ex.Info());
            _conn.Received       += tx => SetStatus($"Data received:{L.f}{tx}");
            _conn.Reconnected    += () => SetStatus("Broken connection successfully reconnected.");
            _conn.Reconnecting   += () => SetStatus("Attempting to restore the connection ...");
            //_conn.StateChanged   += sc => SetStatus($"Connection state changed from ‹{sc.OldState}› to ‹{sc.NewState}›.");
            _conn.StateChanged   += sc => _connectStateChanged?.Raise(ConnectState);
        }


        public void Disconnect()
        {
            try
            {
                if (_conn == null) return;
                var xConn = _conn;
                _conn = null;
                _hub = null;

                Task.Run(() =>
                {
                    try { xConn.Dispose(); }
                    catch { }
                });
            }
            catch { }
        }




        protected override void SetStatus(string statusText)
            => _ui.Send(_ => base.SetStatus(statusText), null);


        private ConnectionStatus GetConnectionState()
        {
            if (_conn == null) return ConnectionStatus.Disconnected;
            switch (_conn.State)
            {
                case ConnectionState.Connecting  : return ConnectionStatus.Connecting;
                case ConnectionState.Connected   : return ConnectionStatus.Connected;
                case ConnectionState.Reconnecting: return ConnectionStatus.Reconnecting;
                case ConnectionState.Disconnected: return ConnectionStatus.Disconnected;
                default                          : return ConnectionStatus.Unknown;
            }
        }
    }
}
