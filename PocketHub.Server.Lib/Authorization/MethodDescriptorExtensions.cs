using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Repo2.Core.ns11.Extensions.StringExtensions;
using System.Collections.Generic;

namespace PocketHub.Server.Lib.Authorization
{
    public static class MethodDescriptorExtensions
    {

        public static List<string> GetRoles<T>(this MethodDescriptor hubMethod)
            where T : AuthorizeAttribute
        {
            var roles = new List<string>();

            foreach (var attr in hubMethod.Attributes)
            {
                var authAttr = attr as T;
                if (authAttr == null) continue;
                if (authAttr.Roles.IsBlank()) continue;
                roles.AddRange(authAttr.Roles.SplitTrim(","));
            }
            return roles;
        }

    }
}
