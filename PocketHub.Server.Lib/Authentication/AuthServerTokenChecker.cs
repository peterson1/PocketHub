using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions;
using Repo2.Core.ns11.RestClients;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Authentication
{
    public class AuthServerTokenChecker
    {
        private bool                          _isCredentialsSet;
        private IR2Credentials                _creds;
        private IR2RestClient                 _rest;
        private Dictionary<string, IUserInfo> _authorized = new Dictionary<string, IUserInfo>();


        public AuthServerTokenChecker(IR2RestClient r2RestClient, IR2Credentials credentials)
        {
            _rest  = r2RestClient;
            _creds = credentials;
        }


        public async Task<bool> IsAuthorized(string userNme, string authTokn)
        {
            SetCredentials(userNme);

            var list = await _rest.List<IUserInfo, UserInfoByAuthToken>(new CancellationToken(), authTokn);
            if (!list.Any()) return false;

            if (list.Count > 1) throw Fault.NonSolo
                ($"‹{typeof(IUserInfo).Name}› for token “{authTokn}”", list.Count);

            var usrInf = list[0];

            if (usrInf.Username != userNme) return false;

            _authorized.Remove(userNme);
            _authorized.Add(userNme, usrInf);

            return true;
        }


        public IUserInfo GetProfile (string userName) => _authorized.GetOrDefault(userName);
        public bool      HasProfile (string userName) => _authorized.ContainsKey(userName);


        private void SetCredentials(string userNme)
        {
            if (_isCredentialsSet) return;

            _rest.SetCredentials(_creds, true);

            _isCredentialsSet = true;
        }
    }
}
