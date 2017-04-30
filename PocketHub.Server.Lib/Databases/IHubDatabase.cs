using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Databases
{
    public interface IHubDatabase<T> : IStatusChanger
    {
        Task<Reply<int>>   CreateNew (SubjectAlterations mods);
        Task<Reply<bool>>  Update    (SubjectAlterations mods);

        Task<Reply<T>>     GetById   (int id);
    }
}
