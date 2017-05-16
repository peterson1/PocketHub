using Repo2.Core.ns11.DataStructures;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceContracts
{
    public interface IObjectDBReader
    {
        Task<Reply<T>> GetById<T>(uint id);
    }
}
