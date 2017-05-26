using PocketHub.Client.Lib.UserInterfaces.Logging;
using PocketHub.Server.Lib.ComponentRegistry;
using PocketHub.Server.Lib.Configuration;
using PocketHub.Server.Lib.MainTabs.ConnectionsTab;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ViewModelTools;
using System;
using System.Collections.Generic;

namespace PocketHub.Server.Lib.MainWindows
{
    public class MainHubWindowVM1 : MainWindowBase
    {
        protected override string CaptionPrefix => Title;

        public MainHubWindowVM1(ActivityLogVM activityLogVM, ServerSettings serverSettings, ServerToggleVM serverToggleVM, ConnectionsTabVM connectionsTabVM, IFileSystemAccesor fs) : base(serverSettings, serverToggleVM, connectionsTabVM, fs)
        {
            foreach (var type in TabTypes)
                AddAsTab(StaticRegistry.Resolve(type) as R2ViewModelBase);

            AddAsTab(activityLogVM);
        }

        public static string      Title     { get; set; }
        public static List<Type>  TabTypes  { get; } = new List<Type>();
    }
}
