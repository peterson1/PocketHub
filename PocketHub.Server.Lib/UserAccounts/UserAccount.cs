﻿//using NullGuard;
using System.Collections.Generic;

namespace PocketHub.Server.Lib.UserAccounts
{
    //[NullGuard(ValidationFlags.None)]
    public class UserAccount
    {
        public uint          Id             { get; set; }
        public string        LoginName      { get; set; }
        public string        FullName       { get; set; }
        public string        ShortName      { get; set; }
        public bool          IsBlocked      { get; set; }
        public string        SaltedKeyHash  { get; set; }
        public string        Remarks        { get; set; }
        public List<string>  Roles          { get; set; }
    }
}
