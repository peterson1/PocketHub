using System.Collections.Generic;

namespace PocketHub.Server.Lib.Authentication
{
    public interface IUserInfo
    {
        int                  UserId    { get; }
        string               Username  { get; }
        IEnumerable<string>  Roles     { get; }
        string               AllRoles  { get; }
    }
}
