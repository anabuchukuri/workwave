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



        public Task<bool> AddRole(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<string> ChangePassword(string userName, string OldPassword, string NewPassword)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetCurrentUser(ClaimsPrincipal claimsUser)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetRoles(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasRole(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Login(string userName, string password, bool rememberMe)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.CheckPasswordAsync(user, password);
            return (user!=null && result) ? user : null;
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> Register(User user, string password, bool signIn = true)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public Task<bool> RemoveRoles(User user, List<string> roleNames)
        {
            throw new NotImplementedException();
        }

    }
}
