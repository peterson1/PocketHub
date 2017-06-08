using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.UserAccounts
{
    public interface IUserAccountsRepo
    {
        Task<UserAccount>            FindAccountAsync        (string loginName);
        UserAccount                  FindAccount             (string loginName);
        UserAccount                  FindAccount             (int userId);
        List<Tuple<uint, string>>    GetIDsAndRemarks        ();
        List<Tuple<string, string>>  GetFullNamesAndRemarks  ();
    }
}
