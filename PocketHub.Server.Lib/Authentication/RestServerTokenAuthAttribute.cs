using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PocketHub.Server.Lib.ComponentRegistry;
using PocketHub.Server.Lib.Logging;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace PocketHub.Server.Lib.Authentication
{
    public class RestServerTokenAuthAttribute : AuthorizeAttribute
    {
        private ActivityLogVM           _log;
        private IR2Credentials          _creds;
        private AuthServerTokenChecker  _authSvr;


        public RestServerTokenAuthAttribute()
        {
            _log     = ComponentRegistryBase.Resolve<ActivityLogVM>();
            _creds   = ComponentRegistryBase.Resolve<IR2Credentials>();
            _authSvr = ComponentRegistryBase.Resolve<AuthServerTokenChecker>();
        }


        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            if (!request.IsNegotiating()) return true;

            var userNme  = request.GetUserName();
            if (userNme.IsBlank()) return false;

            var authTokn = request.GetAuthToken();
            if (authTokn.IsBlank()) return false;

            _log.Info($"Authenticating on [{_creds.BaseURL}] as “{userNme}” ...");

            var ok = _authSvr.IsAuthorized(userNme, authTokn).ConfigureAwait(false).GetAwaiter().GetResult();

            if (!ok) _log.Info("Access denied.");
            return ok;
        }


        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var usrNme = hubIncomingInvokerContext.Hub.Context.Request.GetUserName();
            if (usrNme.IsBlank()) return false;

            return _authSvr.HasProfile(usrNme);
        }
    }
}
