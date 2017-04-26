using PocketHub.Server.Lib.Authentication;
using PropertyChanged;
using System;

namespace PocketHub.Server.Lib.MainTabs.ConnectionsTab
{
    [ImplementPropertyChanged]
    public class ClientRowVM
    {
        public ClientRowVM(string conxnId, IUserInfo identity, string targetHub)
        {
            ConnectionId = conxnId;
            Identity     = identity;
            TargetHub    = targetHub;
        }

        public string        ConnectionId      { get; }
        public IUserInfo     Identity          { get; }
        public string        TargetHub         { get; }
        public string        ConnectionState   { get; set; }
        public string        LastMessage       { get; set; }
        public DateTime      LastUpdate        { get; set; }
    }
}
