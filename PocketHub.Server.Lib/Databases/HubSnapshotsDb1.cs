using PocketHub.Server.Lib.UserAccounts;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.Databases;
using System;

namespace PocketHub.Server.Lib.Databases
{
    internal class HubSnapshotsDb1<T> : SubjectSnapshotsRepoBase<T>
        where T : ISubjectSnapshot, new()
    {
        private IUserAccountsRepo _usrs;
        private Func<UserAccount, string> _actorNameGettr;

        protected override string DbFileName => $@"Databases\{typeof(T).Name}_Snapshots.LiteDB3";
        protected override string CollectionName => "v1";



        internal HubSnapshotsDb1(IFileSystemAccesor fs, ISubjectAlterationsDB subjectAlterationsDB, 
            IUserAccountsRepo userAccountsRepo, Func<UserAccount, string> actorNameGetter)
            : base(fs, subjectAlterationsDB)
        {
            _usrs           = userAccountsRepo;
            _actorNameGettr = actorNameGetter;
        }


        protected override string GetActorName(int actorID)
            => _actorNameGettr(_usrs.FindAccount(actorID));
    }
}
