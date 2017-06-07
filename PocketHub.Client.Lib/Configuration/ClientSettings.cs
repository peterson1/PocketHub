using Repo2.SDK.WPF45.Configuration;
using Repo2.SDK.WPF45.FileSystems;

namespace PocketHub.Client.Lib.Configuration
{
    public class ClientSettings : IHubCredentials
    {
        public string  HubServerURL   { get; set; }
        public string  SharedKey      { get; set; }
        public string  LoginName      { get; set; }
        public string  LoginPassword  { get; set; }


        public static ClientSettings CreateDefault()
            => new ClientSettings
            {
                HubServerURL       = "http://localhost:33301",
                SharedKey          = "????",
            };


        public static ClientSettings LoadFile()
        {
            var fs    = new FileSystemAccesor1();
            var loadr = new BesideExeCfgLoader<ClientSettings>(fs);
            return loadr.Load(ClientSettings.CreateDefault());
        }
    }
}
