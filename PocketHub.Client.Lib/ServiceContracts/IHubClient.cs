﻿using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using System;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.ServiceContracts
{
    public enum ConnectionStatus
    {
        Unknown   = 0,
        Connecting   ,
        Connected    ,
        Reconnecting ,
        Disconnected ,
    }


    public interface IHubClient : IStatusChanger
    {
        event EventHandler<ConnectionStatus> ConnectStateChanged;

        Task<Reply>       Connect       (string hubServerUrl, string username, string authenticationToken);
        void              Disconnect    ();
        bool              IsConnected   { get; }
        ConnectionStatus  ConnectState  { get; }
    }
}