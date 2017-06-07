using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
using Repo2.SDK.WPF45.ViewModelTools;
using System;

namespace PocketHub.Client.Lib.UserInterfaces.Logging
{
    //[ImplementPropertyChanged]
    public class ActivityLogVM : R2ViewModelBase
    {
        public ActivityLogVM()
        {
            UpdateTitle("Logs");
            ShowTrace = true;
        }


        public bool ShowTrace { get; set; }


        public Observables<string> Rows { get; } = new Observables<string>();


        public void Info(string message)
            => AsUI(_ => Rows.Add(AddTimestamp(message)));


        private string AddTimestamp(string message)
            => $"[{DateTime.Now.ToString("hh:mm:sst").ToLower()}]  {message}";


        public void Trace(string message)
        {
            if (ShowTrace)
                AsUI(_ => Rows.Add(AddTimestamp(message)));
        }


        public void Unexpected(Exception ex) => AsUI(_ 
            => Rows.Add(AddTimestamp(ex.Info(true, true))));



        public void Threat(string message) => AsUI(_ 
            => Rows.Add($"!!! THREAT !!!  {AddTimestamp(message)}"));
    }
}
