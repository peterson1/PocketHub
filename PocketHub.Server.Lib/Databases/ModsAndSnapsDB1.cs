using PocketHub.Server.Lib.Logging;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using System;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Databases
{
    public class ModsAndSnapsDB1<T> : StatusChangerN45, IHubDatabase<T>
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


        public async Task<Reply<int>> CreateNew(SubjectAlterations mods)
        {
            SetStatus($"Creating new {TName} record ...");
            int id;
            try
            {
                id = await _modsDB.CreateNewSubject(mods);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Info(true, true));
                return Reply.Error<int>(ex.Info(false, false));
            }
            SetStatus($"Successfully created new {TName} record (id: {id}).");
            return new Reply<int>(id);
        }


        public Task<Reply<bool>> Update(SubjectAlterations mods)
        {
            throw new NotImplementedException();
        }


        public Task<Reply<T>> GetById(int id)
        {
            throw new NotImplementedException();
        }


        private string TName => typeof(T).Name.Replace("DTO", "");
    }
}
