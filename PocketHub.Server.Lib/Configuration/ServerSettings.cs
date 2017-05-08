using Repo2.Core.ns11.Authentication;
using Repo2.SDK.WPF45.Configuration;
using Repo2.SDK.WPF45.FileSystems;

namespace PocketHub.Server.Lib.Configuration
{
    public class ServerSettings : IR2Credentials
    {
        public string  HubServerURL        { get; set; }
        public string  SharedKey           { get; set; }
        public string  AuthServerURL       { get; set; }
        public string  AuthServerThumb     { get; set; }
        public string  AuthServerUsername  { get; set; }
        public string  AuthServerPassword  { get; set; }


        string IR2Credentials.BaseURL              => AuthServerURL;
        string IR2Credentials.CertificateThumb     => AuthServerThumb;
        string IR2Credentials.Username             => AuthServerUsername;
        string IR2Credentials.Password             => AuthServerPassword;
        int    IR2Credentials.CheckIntervalSeconds => 0;


        public static ServerSettings CreateDefault()
            => new ServerSettings
            {
                HubServerURL       = "http://localhost:33301",
                SharedKey          = "????",
                AuthServerURL      = "????",
                AuthServerThumb    = "????",
                AuthServerUsername = "????",
                AuthServerPassword = "????"
            };


        public static ServerSettings LoadFile()
        {
            var fs    = new FileSystemAccesor1();
            var loadr = new BesideExeCfgLoader<ServerSettings>(fs);
            return loadr.Load(ServerSettings.CreateDefault());
        }
    }
}
