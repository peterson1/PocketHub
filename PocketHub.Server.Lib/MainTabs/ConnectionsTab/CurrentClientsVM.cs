using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.UserAccounts;
using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.SDK.WPF45.ViewModelTools;
using System;
using System.Linq;

namespace PocketHub.Server.Lib.MainTabs.ConnectionsTab
{
    //[ImplementPropertyChanged]
    public class CurrentClientsVM : R2ViewModelBase
    {
        public Observables<ClientRowVM> Rows { get; } = new Observables<ClientRowVM>();


        public void AddOrEdit(string connId, UserAccount currentUsr, string hubName, string connectionState)
        {
            var row = Rows.SingleOrDefault(x => x.ConnectionId == connId);
            if (row == null)
            {
                row = new ClientRowVM(connId, currentUsr, hubName);
                AsUI(_ => Rows.Add(row));
            }
            row.ConnectionState = connectionState;
            row.LastUpdate      = DateTime.Now;
        }
    }
}
