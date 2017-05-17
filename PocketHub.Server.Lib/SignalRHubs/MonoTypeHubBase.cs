using PocketHub.Client.Lib.ServiceContracts;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.Databases;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using PocketHub.Server.Lib.UserAccounts;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.FileSystems;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.SignalRHubs
{
    [PocketHubHeaderAuth(Roles = "Authenticated User")]
    public abstract class MonoTypeHubBase<T> : SignalRHubBase, IMonoTypeHub<T>
    {
        private MonoTypeLocalDB<T> _db;


        public MonoTypeHubBase(IFileSystemAccesor fileSystemAccesor,
            MonoTypeLocalDB<T> localRepoBase,
            ActivityLogVM activityLogVM, IUserAccountsRepo userAccountsRepo, CurrentClientsVM currentClientsVM, ServerSettings serverSettings) : base(activityLogVM, userAccountsRepo, currentClientsVM, serverSettings)
        {
            _db = localRepoBase;
        }


        public async Task<Reply<T>> GetById(uint id)
        {
            await Task.Delay(0);
            return new Reply<T>(_db.FindById(id));
        }


        [PocketHubHeaderAuth(Roles = "Migrator")]
        public async Task<Reply<uint>>  Insert (T newRecord)
        {
            await Task.Delay(0);
            return new Reply<uint>(_db.Insert(newRecord));
        }
    }
}
