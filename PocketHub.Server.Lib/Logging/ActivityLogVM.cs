using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.SDK.WPF45.ViewModelTools;
using System;

namespace PocketHub.Server.Lib.Logging
{
    [ImplementPropertyChanged]
    public class ActivityLogVM : R2ViewModelBase
    {
        public Observables<string> Rows { get; } = new Observables<string>();


        public void Info(string message)
            => AsUI(_ => Rows.Add(AddTimestamp(message)));


        private string AddTimestamp(string message)
            => $"[{DateTime.Now.ToString("hh:mm:sst").ToLower()}]  {message}";
    }
}
