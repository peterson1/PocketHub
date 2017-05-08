using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.UserAccounts
{
    public interface IUserAccountsRepo
    {
        Task<UserAccount>  FindAccountAsync  (string loginName);
        UserAccount        FindAccount       (string loginName);
        Dictionary<int, string> GetDictionary();
    }
}
