using Repo2.SDK.WPF45.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.Databases;

namespace PocketHub.Server.Lib.Databases
{
    internal class HubSnapshotsDb1<T> : SubjectSnapshotsRepoBase
    {
        protected override string DbFileName => $@"Databases\{typeof(T).Name}_Snapshots.LiteDB3";
        protected override string CollectionName => "v1";


        internal HubSnapshotsDb1(IFileSystemAccesor fs, ISubjectAlterationsDB subjectAlterationsDB) : base(fs, subjectAlterationsDB)
        {
        }
    }
}
