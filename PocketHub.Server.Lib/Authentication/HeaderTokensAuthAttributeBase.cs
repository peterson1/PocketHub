using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using System;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Authentication
{
    public abstract class HeaderTokensAuthAttributeBase : AuthorizeAttribute
    {
        protected ActivityLogVM _log;

        public HeaderTokensAuthAttributeBase(ActivityLogVM activityLogVM)
        {
            _log = activityLogVM;
        }


        protected abstract Task<bool>  CanConnect (string username, string authToken);
        protected abstract Task<bool>  CanInvoke  (string username, string authToken, MethodDescriptor hubMethod);


        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            if (!request.IsNegotiating()) return true;

            if (!TryGetCredentials(request, out string usrName,
                                            out string authTokn)) return false;
            var ok = false;
            try
            {
                ok = CanConnect(usrName, authTokn).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _log.Info(ex.Info(false, false));
                //return false;  --> client see 401:Unauthorized whatever the error is
                throw ex;     // --> client sees actual HTTP error code
            }

            if (!ok) _log.Info("Connection denied.");
            return ok;
        }


        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext invoker, bool appliesToMethod)
        {
            var request = invoker.Hub.Context.Request;

            if (!TryGetCredentials(request, out string usrName, 
                                            out string authTokn)) return false;
            var ok = false;
            try
            {
                ok = CanInvoke(usrName, authTokn, invoker.MethodDescriptor)
                    .ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _log.Info(ex.Info(false, false));
                //return false;  --> client see 401:Unauthorized whatever the error is
                throw ex;     // --> client sees actual HTTP error code
            }

            if (!ok) _log.Info("Invocation denied.");
            return ok;
        }


        private bool TryGetCredentials(IRequest request, 
            out string usrName, out string authTokn)
        {
            authTokn = null;
            usrName  = request.GetUserName();
            if (usrName.IsBlank()) return false;

            authTokn = request.GetAuthToken();
            if (authTokn.IsBlank()) return false;

            return true;
        }
    }
}
