using PocketHub.Server.Lib.Authentication;
using PocketHub.Server.Lib.UserAccounts;
using PropertyChanged;
using System;

namespace PocketHub.Server.Lib.MainTabs.ConnectionsTab
{
    [ImplementPropertyChanged]
    public class ClientRowVM
    {
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
    }
}
