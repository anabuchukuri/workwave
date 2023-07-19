using Microsoft.AspNetCore.Identity;
using WorkWave.DBModels;
using WorkWave.Services.Abstracts;

namespace WorkWave.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
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
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            return await Task.FromResult(_roleManager.Roles.Select(r => r.Name).ToList());
        }

        /*public async Task<List<User>> GetUsersInRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return new List<User>();

            return await Task.FromResult(_roleManager.Roles.ToList());
        }*/
    }
}
