using Microsoft.AspNet.SignalR;
using System;

namespace PocketHub.Server.Lib.Authentication
{
    public static class HubContextExtensions
    {
        public static string GetUserName(this IRequest req)
            => req.Headers["username"];

        public static string GetAuthToken(this IRequest req)
            => req.Headers["auth-token"];

        public static bool IsNegotiating(this IRequest req)
            => req.LocalPath.EndsWith("/negotiate", StringComparison.OrdinalIgnoreCase);
    }
}
