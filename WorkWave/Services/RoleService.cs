using Microsoft.AspNetCore.Identity;
using WorkWave.DBModels;
using WorkWave.Services.Abstracts;

namespace WorkWave.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleService(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            var role = new Role { Name = roleName };
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' not found." });

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName.ToLower());
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            return await Task.FromResult(_roleManager.Roles.Select(r => r.Name).ToList());
        }

        public async Task<List<User>> GetUsersInRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.ToList();

        }

        public async Task<bool> RemoveRolesFromUser(User user, List<string> roleNames)
        {
            var result = await _userManager.RemoveFromRolesAsync(user, roleNames);
            return result.Succeeded;
        }

        public async Task<bool> AddRoleToUser(User user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<List<string>> GetUserRoles(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> UserHasRole(User user, string roleName)
        {
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            return isInRole;
        }
    }
}
