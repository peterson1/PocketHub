using PocketHub.Client.Lib.ServiceContracts;
using Repo2.Core.ns11.DataStructures;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceProviders
{
    public abstract class MonoTypeClientBase<T> : HubClientBase
    {
        protected override string HubName => $"{typeof(T).Name}Hub";


        public Task<Reply<uint>> Insert(T newRecord)
            => Invoke<uint>(nameof(IMonoTypeHub<T>.Insert), newRecord);


        public Task<Reply<T>> GetById (uint id)
            => Invoke<T>(nameof(IMonoTypeHub<T>.GetById), id);
    }
}
