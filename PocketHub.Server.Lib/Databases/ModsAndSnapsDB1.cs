using PocketHub.Client.Lib.UserInterfaces.Logging;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Databases
{
    public class ModsAndSnapsDB1<T> : StatusChangerN45, IHubDatabase<T>
        where T : ISubjectSnapshot, new()
    {
        private HubAlterationsDb1<T> _modsDB;
        private HubSnapshotsDb1  <T> _snapsDB;
        private ActivityLogVM        _log;

        public ModsAndSnapsDB1(IFileSystemAccesor fs, ActivityLogVM activityLogVM)
        {
            _log     = activityLogVM;
            _modsDB  = new HubAlterationsDb1<T>(fs);
            _snapsDB = new HubSnapshotsDb1<T>(fs, _modsDB);

            _modsDB .StatusChanged += (s, e) => SetStatus(e);
            _snapsDB.StatusChanged += (s, e) => SetStatus(e);
        }


        public async Task<Reply<uint>> CreateNew(SubjectAlterations mods)
        {
            SetStatus($"Creating new ‹{TName}› record ...");
            uint id;
            try
            {
                id = await _modsDB.CreateNewSubject(mods);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Info(true, true));
                return Reply.Error<uint>(ex.Info(false, false));
            }
            SetStatus($"Successfully created new ‹{TName}› (id: {id}).");
            return new Reply<uint>(id);
        }


        public Task<Reply<bool>> Update(SubjectAlterations mods)
        {
            throw new NotImplementedException();
        }


        public Task<Reply<T>> GetById(uint id)
        {
            throw new NotImplementedException();
        }


        public async Task<Reply<List<T>>> GetAll()
        {
            SetStatus($"Querying ALL ‹{TName}› records ...");
            List<T> list;
            try
            {
                list = await _snapsDB.GetAll<T>();
            }
            catch (Exception ex)
            {
                _log.Info(ex.Info(true, true));
                return Reply.Error<List<T>>(ex.Info(false, false));
            }
            SetStatus($"Query returned {list.Count:N0} ‹{TName}› records.");
            return new Reply<List<T>>(list);
        }


        //public async Task<Reply<int>> CountAll()
        //{
        //    await Task.Delay(1);

        //    //var count = await _modsDB.

        //    throw new NotImplementedException();
        //}


        private string TName => typeof(T).Name.Replace("DTO", "");
    }
}
