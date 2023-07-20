using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WorkWave.DBModels;
using WorkWave.Services.Abstracts;

namespace WorkWave.Services
{
    public class AuthService : IAuthService
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        public async Task<IdentityResult> ChangePassword(ClaimsPrincipal userPrincipal, string userName, string OldPassword, string NewPassword)
        {
            userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            return result;
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal claimsUser)
        {
            if (claimsUser == null)
            {
                return null;
            }

            return await _userManager.GetUserAsync(claimsUser);
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var result=await _signInManager.PasswordSignInAsync(userName, password, false, false);
            return result.Succeeded? user:null;
        }

        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<IdentityResult> CreateUser(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}
