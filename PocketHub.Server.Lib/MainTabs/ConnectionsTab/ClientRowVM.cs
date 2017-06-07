using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.UserAccounts;
using PropertyChanged;
using System;
using System.ComponentModel;

namespace PocketHub.Server.Lib.MainTabs.ConnectionsTab
{
    //[ImplementPropertyChanged]
    public class ClientRowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ClientRowVM(string conxnId, UserAccount identity, string targetHub)
        {
            ConnectionId = conxnId;
            Identity     = identity;
            TargetHub    = targetHub;
        }

        public string        ConnectionId      { get; }
        public UserAccount   Identity          { get; }
        public string        TargetHub         { get; }
        public string        ConnectionState   { get; set; }
        public string        LastMessage       { get; set; }
        public DateTime      LastUpdate        { get; set; }

        public string AllRoles => string.Join(", ", Identity?.Roles);
    }
}
