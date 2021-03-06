﻿using Repo2.SDK.WPF45.Configuration;
using Repo2.SDK.WPF45.FileSystems;

namespace PocketHub.Server.Lib.Configuration
{
    public class ServerSettings
    {
        public string    HubServerURL      { get; set; }
        public string    SharedKey         { get; set; }
        public bool      InMemoryDB        { get; set; }
        public bool      AllowBigContent   { get; set; }


        public static ServerSettings CreateDefault()
            => new ServerSettings
            {
                HubServerURL     = "http://localhost:33301",
                SharedKey        = "????",
                InMemoryDB       = false,
                AllowBigContent  = false,
            };


        public static ServerSettings LoadFile()
        {
            var fs    = new FileSystemAccesor1();
            var loadr = new BesideExeCfgLoader<ServerSettings>(fs);
            return loadr.Load(ServerSettings.CreateDefault());
        }
    }
}
