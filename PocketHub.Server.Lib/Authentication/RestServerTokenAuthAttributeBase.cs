using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PocketHub.Server.Lib.Logging;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace PocketHub.Server.Lib.Authentication
{
    public abstract class RestServerTokenAuthAttributeBase : AuthorizeAttribute
    {
        protected abstract ActivityLogVM           Logger   { get; }
        protected abstract AuthServerTokenChecker  AuthSvr  { get; }
        protected abstract IR2Credentials          Creds    { get; }


        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            if (!request.IsNegotiating()) return true;

            var userNme  = request.GetUserName();
            if (userNme.IsBlank()) return false;

            var authTokn = request.GetAuthToken();
            if (authTokn.IsBlank()) return false;

            Logger.Info($"Authenticating on [{Creds.BaseURL}] as “{userNme}” ...");

            var ok = AuthSvr.IsAuthorized(userNme, authTokn).ConfigureAwait(false).GetAwaiter().GetResult();

            if (!ok) Logger.Info("Access denied.");
            return ok;
        }


        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var usrNme = hubIncomingInvokerContext.Hub.Context.Request.GetUserName();
            if (usrNme.IsBlank()) return false;

            return AuthSvr.HasProfile(usrNme);
        }


    }
}
