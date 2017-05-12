using Microsoft.AspNet.SignalR.Hubs;
using PocketHub.Client.Lib.UserInterfaces.Logging;
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
            await Task.Delay(0);
            var acct = GetAuthenticAccount(loginName, authToken);
            return acct != null;
        }


        public async Task<bool> CanInvoke(string loginName, string authToken, MethodDescriptor hubMethod)
        {
            await Task.Delay(0);
            var acct = GetAuthenticAccount(loginName, authToken);
            if (acct == null) return false;
            return HasRoleAccess(acct.Roles, hubMethod);
        }


        private bool HasRoleAccess(List<string> usrRoles, MethodDescriptor hubMethod)
        {
            var methodName  = hubMethod.Name;
            var methodRoles = hubMethod.GetRoles();

            if (!methodRoles.Any())
            {
                _log.Trace($"Method “{methodName}()” has no role restrictions. (Allowing access ...)");
                return true;
            }
            var methodRolesLCase = methodRoles.Select(x => x.ToLower()).ToList();

            if (usrRoles == null)
            {
                _log.Threat("user roles list is NULL (denying...)");
                return false;
            }

            if (!usrRoles.Any())
            {
                _log.Threat("user has no roles (denying...)");
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

            _log.Threat($"no match (denying...)");
            return false;
        }


        private UserAccount GetAuthenticAccount(string loginName, string authToken)
        {
            if (loginName.IsBlank()) return null;
            if (authToken.IsBlank()) return null;

            var acct = _repo.FindAccount(loginName);
            if (acct == null)
            {
                _log.Threat($"No such user: “{loginName}”.");
                return null;
            }

            if (acct.IsBlocked)
            {
                _log.Threat($"Blocked user account: “{loginName}”.");
                return null;
            }

            if (!IsValidToken(acct, authToken)) return null;

            return acct;
        }


        private bool IsValidToken(UserAccount acct, string authToken)
        {
            //var pwd = "abc";
            //_log.Trace($"_cfg.SharedKey: {_cfg.SharedKey}");
            //_log.Trace($"acct.SaltedKeyHash: {acct.SaltedKeyHash}");
            //_log.Trace($"salted key hash for password “{pwd}” : “{(acct.LoginName + pwd + _cfg.SharedKey).SHA1ForUTF8()}”");

            //todo: replace this with HMAC

            var expctd = ( acct.LoginName 
                       +   acct.SaltedKeyHash 
                       +   _cfg.SharedKey ).SHA1ForUTF8();

            if (authToken != expctd)
            {
                //_log.Threat($"Invalid authToken. Expected “{expctd}” but got “{authToken}”.");
                _log.Threat($"“{acct.LoginName}” attempted to login using invalid authToken.");
                return false;
            }
            return true;
        }
    }
}
