using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.Configuration;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.Databases;
using System.IO;

namespace PocketHub.Server.Lib.Databases
{
    public class MonoTypeLocalDB<T> : LocalRepoBase<T>
    {
        public const string DATABASES_DIR = "Databases";

        public MonoTypeLocalDB(IFileSystemAccesor fileSystemAccessor,
                               ActivityLogVM activityLogVM, 
                               ServerSettings serverSettings) 
            : base(serverSettings.InMemoryDB ? null : fileSystemAccessor)
        {
            this.StatusChanged += (s, e) => activityLogVM.Info(e);
        }


        protected override string LocateDatabaseFile(IFileSystemAccesor fs, string filename)
        {
            if (filename.IsBlank()) return string.Empty;

            var path = Path.Combine(fs.CurrentExeDir, DATABASES_DIR, filename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }
    }
}
