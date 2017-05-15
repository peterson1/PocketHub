using Microsoft.AspNet.SignalR;
using PocketHub.Server.Lib.Configuration;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.SDK.WPF45.Encryption;
using System;

namespace PocketHub.Server.Lib.Authentication
{
    public static class HubContextExtensions
    {
        public static string GetUserName(this IRequest req, ServerSettings cfg)
            => Decrypt(req.Headers["username"], cfg.SharedKey);

        public static string GetAuthToken(this IRequest req, ServerSettings cfg)
            => Decrypt(req.Headers["auth-token"], cfg.SharedKey);

        public static bool IsNegotiating(this IRequest req)
            => req.LocalPath.EndsWith("/negotiate", StringComparison.OrdinalIgnoreCase);


        private static string Decrypt(string text, string key)
        {
            if (text.IsBlank()) return string.Empty;
            if (key .IsBlank()) return string.Empty;
            return AESThenHMAC.SimpleDecryptWithPassword(text, key);
        }
    }
}
