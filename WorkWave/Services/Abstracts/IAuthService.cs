using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WorkWave.DBModels;

namespace WorkWave.Services.Abstracts
{
    public interface IAuthService
    {
        /* string roleName = RoleHelper.User*/
        Task<IdentityResult> Register(User user, string password, bool signIn = true);

            Task<bool> Logout();

            Task<User> Login(string userName, string password, bool rememberMe);

            /*Task<bool> IsSignedIn(ClaimsPrincipal claimsUser);

            Task<string> GetUserName(ClaimsPrincipal claimsUser);*/

            Task<User> GetCurrentUser(ClaimsPrincipal claimsUser);
            Task<List<string>> GetRoles(User user);
            Task<bool> HasRole(User user, string roleName);

            Task<bool> AddRole(User user, string roleName);

            Task<bool> RemoveRoles(User user, List<string> roleNames);
            Task<string> ChangePassword(string userName, string OldPassword, string NewPassword);

    }
}
