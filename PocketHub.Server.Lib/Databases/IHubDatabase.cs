using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Databases
{
    public interface IHubDatabase<T> : IStatusChanger
        where T : ISubjectSnapshot, new()
    {
        Task<Reply<uint>>     CreateNew (SubjectAlterations mods);
        Task<Reply<bool>>     Update    (SubjectAlterations mods);
        Task<Reply<T>>        GetById   (uint id) ;
        Task<Reply<List<T>>>  GetAll    ();
        //Task<Reply<int>>      CountAll  ();
    }
}
