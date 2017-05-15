using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.UserAccounts;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using System;
using System.Collections.Generic;

namespace PocketHub.Server.Lib.Databases
{
    public abstract class ModsAndSnapsRepoBase<T> : StatusChangerN45, IHubDatabase<T>
        where T : ISubjectSnapshot, new()
    {
        private HubAlterationsDb1<T> _modsDB;
        private HubSnapshotsDb1  <T> _snapsDB;
        private ActivityLogVM        _log;


        protected virtual string GetActorName(UserAccount usr) => usr?.FullName;


        public ModsAndSnapsRepoBase(IUserAccountsRepo userAccountsRepo, IFileSystemAccesor fs, ActivityLogVM activityLogVM)
        {
            _log     = activityLogVM;
            _modsDB  = new HubAlterationsDb1<T>(fs);
            _snapsDB = new HubSnapshotsDb1<T>(fs, _modsDB, 
                userAccountsRepo, usr => GetActorName(usr));

            _modsDB .StatusChanged += (s, e) => SetStatus(e);
            _snapsDB.StatusChanged += (s, e) => SetStatus(e);
        }


        public Reply<uint> CreateNew(SubjectAlterations mods)
        {
            SetStatus($"Creating new ‹{TName}› record ...");
            uint id;
            try
            {
                id = _modsDB.CreateNewSubject(mods);
            }
            catch (Exception ex)
            {
                _log.Unexpected(ex);
                return Reply.Error<uint>(ex.Info(true, true));
            }
            SetStatus($"Successfully created new ‹{TName}› (id: {id}).");
            return new Reply<uint>(id);
        }



        public Reply<List<T>> GetAll()
        {
            SetStatus($"Querying ALL ‹{TName}› records ...");
            List<T> list;
            try
            {
                list = _snapsDB.GetAll();
            }
            catch (Exception ex)
            {
                _log.Info(ex.Info(true, true));
                return Reply.Error<List<T>>(ex.Info(false, false));
            }
            SetStatus($"Query returned {list.Count:N0} ‹{TName}› records.");
            return new Reply<List<T>>(list);
        }


        public Reply<bool> Update(SubjectAlterations mods)
        {
            SetStatus($"Updating stored ‹{TName}› (id: {mods.SubjectID}) ...");
            bool changd;
            try
            {
                changd = _modsDB.UpdateSubject(mods);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Info(true, true));
                return Reply.Error<bool>(ex.Info(false, false));
            }
            SetStatus($"Successfully updated ‹{TName}›.");
            return new Reply<bool>(changd);
        }


        private string TName => typeof(T).Name.Replace("DTO", "");
    }
}
