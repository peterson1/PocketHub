using PocketHub.Client.Lib.ServiceContracts;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using PocketHub.Server.Lib.UserAccounts;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.SignalRHubs
{
    [PocketHubHeaderAuth(Roles = "Migrator")]
    public class UserAccountHub : SignalRHubBase, IUserAccountReader
    {
        public UserAccountHub(ActivityLogVM activityLogVM, IUserAccountsRepo userAccountsRepo, CurrentClientsVM currentClientsVM, ServerSettings serverSettings) : base(activityLogVM, userAccountsRepo, currentClientsVM, serverSettings)
        {
        }


        [PocketHubHeaderAuth(Roles = "Migrator")]
        public async Task<Reply<List<Tuple<uint, string>>>> GetIDsAndRemarks()
        {
            await Task.Delay(0);
            try
            {
                return new Reply<List<Tuple<uint, string>>>(_usrs.GetIDsAndRemarks());
            }
            catch (Exception ex)
            {
                return Reply.Error<List<Tuple<uint, string>>>(ex.Info());
            }
        }


        [PocketHubHeaderAuth(Roles = "Migrator")]
        public async Task<Reply<List<Tuple<string, string>>>> GetFullNamesAndRemarks()
        {
            await Task.Delay(0);
            try
            {
                return new Reply<List<Tuple<string, string>>>(_usrs.GetFullNamesAndRemarks());
            }
            catch (Exception ex)
            {
                return Reply.Error<List<Tuple<string, string>>>(ex.Info());
            }
        }
    }
}
