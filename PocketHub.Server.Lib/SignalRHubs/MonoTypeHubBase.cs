using PocketHub.Client.Lib.ServiceContracts;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.Databases;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using PocketHub.Server.Lib.UserAccounts;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.FileSystems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.SignalRHubs
{
    [PocketHubHeaderAuth(Roles = "Authenticated User")]
    public abstract class MonoTypeHubBase<T> : SignalRHubBase, IMonoTypeHub<T>
    {
        protected MonoTypeLocalDB<T> _db;


        public MonoTypeHubBase(IFileSystemAccesor fileSystemAccesor,
            MonoTypeLocalDB<T> localRepoBase,
            ActivityLogVM activityLogVM, IUserAccountsRepo userAccountsRepo, CurrentClientsVM currentClientsVM, ServerSettings serverSettings) : base(activityLogVM, userAccountsRepo, currentClientsVM, serverSettings)
        {
            _db = localRepoBase;
        }


        [PocketHubHeaderAuth(Roles = "Migrator")]
        public Task<Reply<uint>> Insert(T newRecord)
            => Query(_ => _.Insert(newRecord));


        [PocketHubHeaderAuth(Roles = "Migrator")]
        public Task<Reply<uint>> BatchInsert(IEnumerable<T> newRecords)
            => Query(_ => _.BatchInsert(newRecords));


        [PocketHubHeaderAuth(Roles = "Migrator")]
        public Task<Reply<uint>> DeleteAll() => Query(_ => _.DeleteAll());


        public Task<Reply<T>>       GetById  (uint id) => Query(_ => _.FindById(id));
        public Task<Reply<List<T>>> GetAll   ()        => Query(_ => _.FindAll());
        public Task<Reply<uint>>    CountAll ()        => Query(_ => _.CountAll());


        public Task<Reply<List<T>>> GetByDates(DateTime startDate, DateTime endDate)
            => Query(_ => _.FindByDates(startDate, endDate));



        protected async Task<Reply<TOut>> Query <TOut>(Func<MonoTypeLocalDB<T>, TOut> query)
        {
            await Task.Delay(0);
            try
            {
                return new Reply<TOut>(query.Invoke(_db));
            }
            catch (Exception ex)
            {
                return Reply.Error<TOut>(ex.Info(true, true));
            }
        }
    }
}
