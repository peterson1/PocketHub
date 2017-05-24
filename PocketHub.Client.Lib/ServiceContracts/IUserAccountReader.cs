using Repo2.Core.ns11.DataStructures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceContracts
{
    public interface IUserAccountReader
    {
        Task<Reply<List<Tuple<uint, string>>>>  GetIDsAndRemarks  ();
    }
}
