using Microsoft.AspNet.SignalR.Hubs;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.ComponentRegistry;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Authentication
{
    public class RestServerTokenAuthAttribute : HeaderTokensAuthAttributeBase
    {
        private AuthServerTokenChecker  _authSvr;


        public RestServerTokenAuthAttribute()
            : base(ComponentRegistryBase.Resolve<ActivityLogVM>())
        {
            _authSvr = ComponentRegistryBase.Resolve<AuthServerTokenChecker>();
        }


        protected override Task<bool> CanConnect(string loginName, string authToken)
            => _authSvr.IsAuthorized(loginName, authToken);


        protected override async Task<bool> CanInvoke(string loginName, string authToken, MethodDescriptor hubMethod)
        {
            await Task.Delay(0);
            return _authSvr.HasProfile(loginName);
        }
    }
}
