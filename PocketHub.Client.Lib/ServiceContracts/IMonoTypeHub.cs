using Repo2.Core.ns11.DataStructures;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceContracts
{
    public interface IMonoTypeHub<T>
    {
        Task<Reply<uint>>  Insert   (T newRecord);
        Task<Reply<T>>     GetById  (uint id);
    }
}
