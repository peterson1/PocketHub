using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Extensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.Databases;
using System.Collections.Generic;

namespace PocketHub.Server.Lib.Databases
{
    internal class HubSnapshotsDb1<T> : SubjectSnapshotsRepoBase<T>
        where T : ISubjectSnapshot, new()
    {
        protected override string DbFileName => $@"Databases\{typeof(T).Name}_Snapshots.LiteDB3";
        protected override string CollectionName => "v1";


        internal Dictionary<int, string>  ActorsDictionary { private get; set; }


        internal HubSnapshotsDb1(IFileSystemAccesor fs, ISubjectAlterationsDB subjectAlterationsDB) : base(fs, subjectAlterationsDB)
        {
        }


        protected override string GetActorName(int actorID)
            => this.ActorsDictionary.GetOrDefault(actorID);
    }
}
