using PocketHub.Server.Lib.ComponentRegistry;
using PocketHub.Server.Lib.Configuration;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.InputCommands;
using System;
using System.ComponentModel;
using System.Reflection;

namespace PocketHub.Server.Lib.MainTabs.ConnectionsTab
{
    //[ImplementPropertyChanged]
    public class ServerToggleVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private      EventHandler _serverStarted;
        public event EventHandler  ServerStarted
        {
            add    { _serverStarted -= value; _serverStarted += value; }
            remove { _serverStarted -= value; }
        }

        private      EventHandler _serverStopped;
        public event EventHandler  ServerStopped
        {
            add    { _serverStopped -= value; _serverStopped += value; }
            remove { _serverStopped -= value; }
        }

        private ServerSettings _cfg;
        private IDisposable    _webApp;
        private IWebAppStarter _startr;

        public ServerToggleVM(ServerSettings serverSettings, IWebAppStarter webAppStarter)
        {
            _cfg           = serverSettings;
            _startr        = webAppStarter;
            StartServerCmd = R2Command.Relay(StartServer, _ => !IsServerStarted, "Start Server");
            StopServerCmd  = R2Command.Relay(StopServer , _ =>  IsServerStarted, "Stop Server");
        }

        public IR2Command  StartServerCmd  { get; } 
        public IR2Command  StopServerCmd   { get; }

        public bool IsServerStarted => _webApp != null;


        private void StartServer()
        {
            try
            {
                //_webApp = WebApp.Start<T>(_cfg.HubServerURL);
                _webApp = _startr.StartWebApp(_cfg.HubServerURL);
            }
            catch (TargetInvocationException ex)
            {
                Alerter.ShowError($"Unable to start server at {_cfg.HubServerURL}", 
                                  $"You may need to pick a different port number.{L.F}{ex.Info()}");

                Alerter.ShowInfo("Other Options:",
                    $"1.)  Run the server as Administrator.{L.f}"
                    + "2.)  netsh http add urlacl http://*:123456/ user=EVERYONE");
            }
            if (IsServerStarted) _serverStarted.Raise();
        }


        private void StopServer()
        {
            try   { _webApp?.Dispose(); }
            catch { }
            _webApp = null;
            _serverStopped.Raise();
        }
    }
}
