using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Databases
{
    public interface IHubDatabase<T> : IStatusChanger
        where T : ISubjectSnapshot, new()
    {
        Reply<uint>     CreateNew (SubjectAlterations mods);
        Reply<List<T>>  GetAll    ();
        Reply<bool>     Update    (SubjectAlterations mods);
        //Reply<T>        GetById   (uint id) ;
        //Task<Reply<int>>      CountAll  ();
    }
}
