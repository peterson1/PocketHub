using Repo2.Core.ns11.Authentication;
using Repo2.SDK.WPF45.Configuration;
using Repo2.SDK.WPF45.FileSystems;

namespace PocketHub.Client.Lib.Configuration
{
    public class ClientSettings : IR2Credentials
    {
        public string  HubServerURL        { get; set; }
        public string  AuthServerURL       { get; set; }
        public string  AuthServerThumb     { get; set; }
        public string  AuthServerUsername  { get; set; }
        public string  AuthServerPassword  { get; set; }


        string IR2Credentials.BaseURL              => AuthServerURL;
        string IR2Credentials.CertificateThumb     => AuthServerThumb;
        string IR2Credentials.Username             => AuthServerUsername;
        string IR2Credentials.Password             => AuthServerPassword;
        int    IR2Credentials.CheckIntervalSeconds => 0;


        public static ClientSettings CreateDefault()
            => new ClientSettings
            {
                HubServerURL       = "http://localhost:33301",
                AuthServerURL      = "????",
                AuthServerThumb    = "????",
                AuthServerUsername = "????",
                AuthServerPassword = "????"
            };


        public static ClientSettings LoadFile()
        {
            var fs    = new FileSystemAccesor1();
            var loadr = new BesideExeCfgLoader<ClientSettings>(fs);
            return loadr.Load(ClientSettings.CreateDefault());
        }
    }
}
