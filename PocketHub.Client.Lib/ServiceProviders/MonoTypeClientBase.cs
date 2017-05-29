using PocketHub.Client.Lib.ServiceContracts;
using Repo2.Core.ns11.DataStructures;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace PocketHub.Client.Lib.ServiceProviders
{
    public abstract class MonoTypeClientBase<T> : HubClientBase, IMonoTypeHub<T>
    {
        protected override string HubName 
            => $"{typeof(T).Name}Hub".Replace("DTO", "");


        public Task<Reply<uint>> Insert(T newRecord)
            => Invoke<uint>(nameof(IMonoTypeHub<T>.Insert), newRecord);


        public Task<Reply<T>> GetById (uint id)
            => Invoke<T>(nameof(IMonoTypeHub<T>.GetById), id);


        public Task<Reply<uint>> CountAll()
            => Invoke<uint>(nameof(IMonoTypeHub<T>.CountAll));


        public Task<Reply<List<T>>> GetAll()
            => GetList<T>(nameof(IMonoTypeHub<T>.GetAll));


        public Task<Reply<uint>> DeleteAll()
            => Invoke<uint>(nameof(IMonoTypeHub<T>.DeleteAll));


        public Task<Reply<uint>> BatchInsert(IEnumerable<T> newRecords)
            => Invoke<uint>(nameof(IMonoTypeHub<T>.BatchInsert), newRecords);


        public Task<Reply<List<T>>> GetByDates(DateTime startDate, DateTime endDate)
            => GetList<T>(nameof(IMonoTypeHub<T>.GetByDates), startDate, endDate);
    }
}
