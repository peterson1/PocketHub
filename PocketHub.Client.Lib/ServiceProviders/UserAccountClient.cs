using PocketHub.Client.Lib.ServiceContracts;
using Repo2.Core.ns11.DataStructures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceProviders
{
    public class UserAccountClient : HubClientBase, IUserAccountReader
    {
        protected override string HubName => "UserAccountHub";


        public Task<Reply<List<Tuple<uint, string>>>> GetIDsAndRemarks()
            => GetList<Tuple<uint, string>>(nameof(IUserAccountReader.GetIDsAndRemarks));
    }
}
