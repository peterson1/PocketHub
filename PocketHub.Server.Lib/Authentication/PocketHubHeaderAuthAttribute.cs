using Microsoft.AspNet.SignalR.Hubs;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authorization;
using PocketHub.Server.Lib.ComponentRegistry;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Authentication
{
    public class PocketHubHeaderAuthAttribute : HeaderTokensAuthAttributeBase
    {
        private IUserAuthChecker _checkr;

        public PocketHubHeaderAuthAttribute() 
            : base(ComponentRegistryBase.Resolve<ActivityLogVM>())
        {
            _checkr = ComponentRegistryBase.Resolve<IUserAuthChecker>();
        }


        protected override Task<bool> CanConnect(string loginName, string authToken)
            => _checkr.IsValidCredentials(loginName, authToken);


        protected override Task<bool> CanInvoke(string loginName, string authToken, MethodDescriptor hubMethod)
            => _checkr.CanInvoke(loginName, authToken, hubMethod);
    }
}
