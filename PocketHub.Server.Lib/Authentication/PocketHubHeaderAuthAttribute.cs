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
        private ServerSettings   _cfg;

        public PocketHubHeaderAuthAttribute() 
            : base(ComponentRegistryBase.Resolve<ActivityLogVM>())
        {
            _cfg    = ComponentRegistryBase.Resolve<ServerSettings>();
            _checkr = ComponentRegistryBase.Resolve<IUserAuthChecker>();
        }


        protected override Task<bool> CanConnect(string loginName, string authToken)
        {
            var usr = Decrypt(loginName);
            var tkn = Decrypt(authToken);
            _log.Info($"Authenticating as “{usr}” ...");

            return _checkr.IsValidCredentials(usr, tkn);
        }


        protected override Task<bool> CanInvoke(string loginName, string authToken, MethodDescriptor hubMethod)
            => _checkr.CanInvoke(Decrypt(loginName), Decrypt(authToken), hubMethod);


        private string Decrypt(string text)
            => AESThenHMAC.SimpleDecryptWithPassword(text, _cfg.SharedKey);
    }
}
