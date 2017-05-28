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
using System;
using System.Collections.Generic;
using Repo2.Core.ns11.Exceptions;

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


        public async Task<Reply<List<T>>> GetAll()
        {
            await Task.Delay(0);
            return new Reply<List<T>>(_db.FindAll());
        }


        [PocketHubHeaderAuth(Roles = "Migrator")]
        public async Task<Reply<uint>>  Insert (T newRecord)
        {
            await Task.Delay(0);
            try
            {
                return new Reply<uint>(_db.Insert(newRecord));
            }
            catch (Exception ex)
            {
                return Reply.Error<uint>(ex.Info(true, true));
            }
        }


        [PocketHubHeaderAuth(Roles = "Migrator")]
        public async Task<Reply<uint>> BatchInsert(IEnumerable<T> newRecords)
        {
            await Task.Delay(0);
            try
            {
                return new Reply<uint>(_db.BatchInsert(newRecords));
            }
            catch (Exception ex)
            {
                return Reply.Error<uint>(ex.Info(true, true));
            }
        }


        [PocketHubHeaderAuth(Roles = "Migrator")]
        public async Task<Reply<uint>> DeleteAll()
        {
            await Task.Delay(0);
            return new Reply<uint>(_db.DeleteAll());
        }


        public async Task<Reply<uint>> CountAll()
        {
            await Task.Delay(0);
            return new Reply<uint>(_db.CountAll());
        }


        public async Task<Reply<List<T>>> GetByDates(DateTime startDate, DateTime endDate)
        {
            await Task.Delay(0);
            return new Reply<List<T>>(_db.FindByDates(startDate, endDate));
        }
    }
}
