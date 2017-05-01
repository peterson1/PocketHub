using Repo2.Core.ns11.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.Authentication
{
    public class AuthServerTokenSetter
    {
        public async Task<Reply<string>> PostNewToken()
        {
            await Task.Delay(1);
            return new Reply<string>("abc-123");
        }
    }
}
