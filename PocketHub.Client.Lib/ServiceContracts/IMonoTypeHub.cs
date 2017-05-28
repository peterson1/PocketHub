using Repo2.Core.ns11.DataStructures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceContracts
{
    public interface IMonoTypeHub<T>
    {
        Task<Reply<uint>>     Insert       (T newRecord);
        Task<Reply<uint>>     BatchInsert  (IEnumerable<T> newRecords);
        Task<Reply<T>>        GetById      (uint id);
        Task<Reply<uint>>     CountAll     ();
        Task<Reply<List<T>>>  GetAll       ();
        Task<Reply<List<T>>>  GetByDates   (DateTime startDate, DateTime endDate);
        Task<Reply<uint>>     DeleteAll    ();
    }
}
