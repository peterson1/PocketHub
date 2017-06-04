using Microsoft.AspNet.SignalR.Client;
using PocketHub.Client.Lib.ServiceContracts;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.SDK.WPF45.ChangeNotification;
using Repo2.SDK.WPF45.Encryption;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceProviders
{
    public abstract class HubClientBase : StatusChangerN45, IHubClient
    {
        private      EventHandler _connected;
        public event EventHandler  Connected
        {
            add    { _connected -= value; _connected += value; }
            remove { _connected -= value; }
        }

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


        protected abstract string HubName { get; }

        public bool              IsConnected   => _conn?.State == ConnectionState.Connected;
        public ConnectionStatus  ConnectState  => GetConnectionState();



        public async Task<Reply> Connect(string url, string usr, string rawPassword, string sharedKey)
        {
            if (IsConnected) return Reply.Success();

            Disconnect();

            SetStatus($"Connecting to [{url}] as “{usr}” ...");
            var tkn = ComposeAuthToken(usr, rawPassword, sharedKey);

            _conn = new HubConnection(url);
            _conn.Headers.Add("username"  , Encrypt(usr, sharedKey));
            _conn.Headers.Add("auth-token", Encrypt(tkn, sharedKey));

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
                //return Reply.Error($"[{(int)ex.Response.StatusCode}] {ex.Response.ReasonPhrase}");
                var rep = Reply.Error($"[{(int)ex.Response.StatusCode}] {ex.Response.ReasonPhrase}");
                rep.DetailedError = ex.Info(true, true);
                return rep;
            }
            catch (Exception ex)
            {
                return Reply.Error(ex.Info(true, true));
            }

            if (IsConnected)
            {
                SetStatus("Successfully connected to server.");
                OnConnected();
                _connected?.Raise();
                return Reply.Success();
            }
            else
            {
                SetStatus("Failed to connect to server.");
                return Reply.Error("Failed to connect.");
            }
        }


        private string Encrypt(string text, string key)
            => AESThenHMAC.SimpleEncryptWithPassword(text, key);


        private string ComposeAuthToken(string loginName, string rawPassword, string sharedKey)
        {
            if (loginName  .IsBlank()) throw Fault.BlankText("Login Name");
            if (rawPassword.IsBlank()) throw Fault.BlankText("Password"  );
            if (sharedKey  .IsBlank()) throw Fault.BlankText("Shared Key");
            if (sharedKey.Length < 12) throw Fault.BadArg(nameof(sharedKey), "be at least 12 characters");

            var saltdKeyHash = (loginName + rawPassword + sharedKey).SHA1ForUTF8();
            return (loginName + saltdKeyHash + sharedKey).SHA1ForUTF8();
        }


        protected async Task<Reply> Invoke(string methodName, params object[] args)
        {
            if (!IsConnected) throw Fault.BadCall(nameof(Connect));

            SetStatus($"Invoking ‹{HubName}›.{methodName}() ...");
            Reply rep;
            try
            {
                rep = await _hub.Invoke<Reply>(methodName, args);
            }
            catch (Exception ex)
            {
                return Reply.Error(ex.Info(true, true));
            }
            var msg = rep.Failed ? rep.ErrorsText
                    : $"Successfully invoked {methodName}().";

            SetStatus(msg);
            return rep;
        }


        protected async Task<Reply<T>> Invoke<T>(string methodName, params object[] args)
        {
            if (!IsConnected) throw Fault.BadCall(nameof(Connect));

            SetStatus($"Invoking ‹{HubName}›.{methodName}() ...");
            Reply<T> rep;
            try
            {
                rep = await _hub.Invoke<Reply<T>>(methodName, args);
            }
            catch (Exception ex)
            {
                return Reply.Error<T>(ex.Info(true, true));
            }
            var msg = rep.Failed ? rep.ErrorsText
                    : $"Successfully returned ‹{typeof(T).Name}› [{rep.Result}]";

            SetStatus(msg);
            return rep;
        }


        protected async Task<Reply<List<T>>> GetList <T>(string methodName, params object[] args)
        {
            if (!IsConnected) throw Fault.BadCall(nameof(Connect));

            SetStatus($"Getting list via ‹{HubName}›.{methodName}() ...");
            Reply<List<T>> rep;
            try
            {
                rep = await _hub.Invoke<Reply<List<T>>>(methodName, args);
            }
            catch (Exception ex)
            {
                return Reply.Error<List<T>>(ex.Info(true, true));
            }

            var msg = rep.Failed ? rep.ErrorsText
                    : $"Found {rep.Result.Count:N0} ‹{typeof(T).Name}› records.";

            SetStatus(msg);
            return rep;
        }


        /// <summary>
        /// Called by base class right after establishing hub connection.
        /// Safe to override. Base method implementation is empty.
        /// </summary>
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
