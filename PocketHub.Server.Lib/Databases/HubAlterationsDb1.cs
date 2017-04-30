using Repo2.SDK.WPF45.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repo2.Core.ns11.FileSystems;

namespace PocketHub.Server.Lib.Databases
{
    internal class HubAlterationsDb1<T> : SubjectAlterationsRepoBase
    {
        protected override string DbFileName     => $@"Databases\{typeof(T).Name}_Alterations.LiteDB3";
        protected override string CollectionName => "v1";


        internal HubAlterationsDb1(IFileSystemAccesor fs) : base(fs)
        {
        }

    }
}
