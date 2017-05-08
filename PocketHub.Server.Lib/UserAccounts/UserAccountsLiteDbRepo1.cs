using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Repo2.SDK.WPF45.Serialization;

namespace PocketHub.Server.Lib.UserAccounts
{
    public class UserAccountsLiteDbRepo1 : IUserAccountsRepo
    {
        private IFileSystemAccesor _fs;
        private BsonMapper         _bMapr;
        private string             _dbPath;


        public const string COMMON_DIR      = "Common";
        public const string DB_FILENAME     = "UsersRepo1.LiteDB3";
        public const string COLLECTION_NAME = "v1";


        public UserAccountsLiteDbRepo1(IFileSystemAccesor fileSystemAccesor)
        {
            _fs    = fileSystemAccesor;
            _bMapr = new BsonMapper();

            _bMapr.RegisterAutoId<uint>(v => v == 0,
                (db, col) => (uint)db.Count(col) + 1);
        }


        private void CreateSeedRecordsIfEmpty()
        {
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<UserAccount>(COLLECTION_NAME);
                if (col.Count() > 0) return;

                var seedRecs = _fs.ReadDesktopJsonFile<List<UserAccount>>("users_seed.json");
                using (var trans = db.BeginTrans())
                {
                    foreach (var rec in seedRecs)
                    {
                        col.Insert(rec);
                    }
                    trans.Commit();
                }
            }
        }


        private LiteDatabase CreateConnection()
        {
            if (_dbPath.IsBlank())
                _dbPath = LocateDatabaseFile();

            return new LiteDatabase(ConnectString.LiteDB(_dbPath), _bMapr);
        }


        private string LocateDatabaseFile()
        {
            var baseDir = _fs.ParentDir(_fs.CurrentExeDir);
            var path    = Path.Combine(baseDir, COMMON_DIR, DB_FILENAME);
            _fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        public Dictionary<int, string> GetDictionary()
        {
            var dict = new Dictionary<int, string>();
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<UserAccount>(COLLECTION_NAME);

                foreach (var usr in col.FindAll())
                    dict.Add((int)usr.Id, usr.FullName);
            }
            return dict;
        }


        public UserAccount FindAccount(string loginName)
        {
            CreateSeedRecordsIfEmpty();

            using (var db = CreateConnection())
            {
                var col = db.GetCollection<UserAccount>(COLLECTION_NAME);
                col.EnsureIndex(x => x.LoginName, true);

                var matches = col.Find(x => x.LoginName == loginName).ToList();
                if (!matches.Any()) return null;

                if (matches.Count > 1)
                    throw Fault.NonSolo($"User accounts with login name: “{loginName}”", matches.Count);

                return matches.Single();
            }
        }


        public async Task<UserAccount> FindAccountAsync(string loginName)
        {
            await Task.Delay(0);
            return FindAccount(loginName);
        }


        //private UserAccount CreateSeedRecord1()
        //    => new UserAccount
        //    {
        //        LoginName     = "test_user_1",
        //        FullName      = "Test User 1",
        //        ShortName     = "usr1",
        //        IsBlocked     = false,
        //        SaltedKeyHash = ("test_user_1" + "abc").SHA1ForUTF8(),
        //        Roles         = new List<string> { "r1" },
        //    };


        //private UserAccount CreateSeedRecord2()
        //    => new UserAccount
        //    {
        //        LoginName     = "test_user_2",
        //        FullName      = "Test User 2",
        //        ShortName     = "usr2",
        //        IsBlocked     = false,
        //        SaltedKeyHash = ("test_user_2" + "def").SHA1ForUTF8(),
        //        Roles         = new List<string> { "r2" },
        //    };
    }
}
