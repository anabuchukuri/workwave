using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WorkWave.DBModels;

namespace WorkWave.Services.Abstracts
{
    public interface IAuthService
    {
            Task<IdentityResult> CreateUser(User user, string password);
            Task<bool> Logout();
            Task<User> Login(string userName, string password);
            Task<User> GetCurrentUser(ClaimsPrincipal claimsUser);
            Task<IdentityResult> ChangePassword(string userName, string OldPassword, string NewPassword);

    }
}
