using Microsoft.AspNet.SignalR.Hubs;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authorization;
using PocketHub.Server.Lib.ComponentRegistry;
using PocketHub.Server.Lib.Configuration;
using Repo2.SDK.WPF45.Encryption;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Authentication
{
    public class PocketHubHeaderAuthAttribute : HeaderTokensAuthAttributeBase
    {
        private IUserAuthChecker _checkr;

        public PocketHubHeaderAuthAttribute() 
            : base(StaticRegistry.Resolve<ActivityLogVM>(),
                   StaticRegistry.Resolve<ServerSettings>())
        {
            _checkr = StaticRegistry.Resolve<IUserAuthChecker>();
        }


        protected override Task<bool> CanConnect(string usr, string tkn)
        {
            _log.Info($"Authenticating as “{usr}” ...");
            return _checkr.IsValidCredentials(usr, tkn);
        }


        protected override Task<bool> CanInvoke(string loginName, string authToken, MethodDescriptor hubMethod)
            => _checkr.CanInvoke(loginName, authToken, hubMethod);

    }
}
