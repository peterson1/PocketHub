using System;

namespace PocketHub.Server.Lib.ComponentRegistry
{
    public interface IWebAppStarter
    {
        IDisposable  StartWebApp  (string hubServerUrl);
    }
}
