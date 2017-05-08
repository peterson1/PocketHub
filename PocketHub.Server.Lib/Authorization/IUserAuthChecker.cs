using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace PocketHub.Server.Lib.Authorization
{
    public interface IUserAuthChecker
    {
        Task<bool>  IsValidCredentials  (string loginName, string authToken);
        Task<bool>  CanInvoke           (string loginName, string authToken, MethodDescriptor hubMethod);
    }
}
