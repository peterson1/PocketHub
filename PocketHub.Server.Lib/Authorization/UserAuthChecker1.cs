using Microsoft.AspNet.SignalR.Hubs;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.UserAccounts;
using Repo2.Core.ns11.Extensions.StringExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Authorization
{
    public class UserAuthChecker1 : IUserAuthChecker
    {
        private IUserAccountsRepo _repo;
        private ServerSettings    _cfg;
        private ActivityLogVM     _log;

        public UserAuthChecker1(IUserAccountsRepo userAccountsRepo, ServerSettings serverSettings, ActivityLogVM activityLogVM)
        {
            _repo = userAccountsRepo;
            _cfg  = serverSettings;
            _log  = activityLogVM;
        }


        public async Task<bool> IsValidCredentials(string loginName, string authToken)
        {
            var auth = await GetAuthenticAccount(loginName, authToken);
            return auth != null;
        }


        public async Task<bool> CanInvoke(string loginName, string authToken, MethodDescriptor hubMethod)
        {
            var acct = await GetAuthenticAccount(loginName, authToken);
            if (acct == null) return false;
            return HasRoleAccess(acct.Roles, hubMethod.GetRoles<PocketHubHeaderAuthAttribute>());
        }


        private bool HasRoleAccess(List<string> usrRoles, List<string> methodRoles)
        {
            if (!methodRoles.Any())
            {
                _log.Trace("method has no roles restriction (allowing...)");
                return true;
            }
            var methodRolesLCase = methodRoles.Select(x => x.ToLower()).ToList();

            if (usrRoles == null)
            {
                _log.Trace("user roles list is NULL (denying...)");
                return false;
            }

            if (!usrRoles.Any())
            {
                _log.Trace("user has no roles (denying...)");
                return false;
            }

            var usrRolesLCase = usrRoles.Select(x => x.ToLower()).ToList();

            foreach (var usrRole in usrRolesLCase)
            {
                if (methodRolesLCase.Contains(usrRole))
                {
                    _log.Trace($"match: “{usrRole}” (allowing...)");
                    return true;
                }
            }

            _log.Trace($"no match (denying...)");
            return false;
        }


        private async Task<UserAccount> GetAuthenticAccount(string loginName, string authToken)
        {
            if (loginName.IsBlank() || authToken.IsBlank()) return null;
            var acct = await _repo.FindAccount(loginName);
            if (acct == null) return null;
            if (acct.IsBlocked) return null;
            if (!IsValidToken(acct, authToken)) return null;
            return acct;
        }


        private bool IsValidToken(UserAccount acct, string authToken)
        {
            //todo: replace this with HMAC
            var expctd = ( acct.LoginName 
                       +   acct.SaltedKeyHash 
                       +   _cfg.SharedKey ).SHA1ForUTF8();

            return authToken == expctd;
        }
    }
}
