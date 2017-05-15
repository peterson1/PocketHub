using Repo2.SDK.WPF45.Configuration;
using Repo2.SDK.WPF45.FileSystems;

namespace PocketHub.Client.Lib.Configuration
{
    public class TestClientSettings : ClientSettings
    {
        public string  TestLogin     { get; set; }
        public string  TestPassword  { get; set; }


        public static TestClientSettings GetDefault()
            => new TestClientSettings
            {
                HubServerURL = "http://localhost:33301",
                SharedKey    = "????",
                TestLogin    = "????",
                TestPassword = "????",
            };


        public static TestClientSettings ReadFile()
        {
            var fs    = new FileSystemAccesor1();
            var loadr = new BesideExeCfgLoader<TestClientSettings>(fs);
            return loadr.Load(TestClientSettings.GetDefault());
        }
    }
}
