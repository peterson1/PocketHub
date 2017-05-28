using LiteDB;
using PocketHub.Client.Lib.UserInterfaces.Logging;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.Databases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.UserAccounts
{
    public class UserAccountsLocalRepo1 : LocalRepoBase<UserAccount>, IUserAccountsRepo
    {
        public const string COMMON_DIR = "Common";

        private ActivityLogVM _log;


        public UserAccountsLocalRepo1(ActivityLogVM activityLogVM, 
            IFileSystemAccesor fileSystemAccessor) : base(fileSystemAccessor)
        {
            IsSeedEnabled = true;

            _log = activityLogVM;
            base.StatusChanged += (s, e) => _log.Info(e);
        }



        public UserAccount FindAccount(string loginName)
        {
            using (var db = ConnectToDB(out LiteCollection<UserAccount> col))
            {
                col.EnsureIndex(x => x.LoginName, true);

                var matches = col.Find(x => x.LoginName == loginName).ToList();
                if (!matches.Any())
                    matches = FindByLoginNameIgnoreCase(loginName, col);

                if (!matches.Any()) return null;

                if (matches.Count > 1)
                    throw Fault.NonSolo($"User accounts with login name: “{loginName}”", matches.Count);

                return matches.Single();
            }
        }


        private List<UserAccount> FindByLoginNameIgnoreCase(string loginName, LiteCollection<UserAccount> col)
        {
            foreach (var usr in col.FindAll())
            {
                if (usr.LoginName.ToLower() == loginName.ToLower())
                    return new List<UserAccount> { usr };
            }
            return new List<UserAccount>();
        }


        protected override string LocateDatabaseFile(IFileSystemAccesor fs, string filename)
        {
            var baseDir = fs.ParentDir(fs.CurrentExeDir);
            var path = Path.Combine(baseDir, COMMON_DIR, filename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        public List<Tuple<uint, string>> GetIDsAndRemarks()
            => FindAll().Select(usr 
                => new Tuple<uint, string>(usr.Id, usr.Remarks)).ToList();


        public async Task<UserAccount> FindAccountAsync(string loginName)
        {
            await Task.Delay(0);
            return FindAccount(loginName);
        }


        public UserAccount FindAccount(int userId) 
            => base.FindById((uint)userId);


        public override void EnsureIndeces(LiteCollection<UserAccount> col)
        {
            col.EnsureIndex(x => x.LoginName, true);
        }


        protected override string GetJsonSeedFilename() => "users_seed.json";
    }
}
