using Repo2.Core.ns11.DataStructures;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceContracts
{
    public interface IObjectDBWriter
    {
        Task<Reply<uint>>  Insert <T> (T newObject);
    }
}
