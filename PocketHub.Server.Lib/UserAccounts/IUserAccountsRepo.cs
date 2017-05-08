using System.Threading.Tasks;

namespace PocketHub.Server.Lib.UserAccounts
{
    public interface IUserAccountsRepo
    {
        Task<UserAccount>  FindAccount  (string loginName);
    }
}
