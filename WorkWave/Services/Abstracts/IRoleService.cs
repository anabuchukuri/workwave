using Microsoft.AspNetCore.Identity;
using WorkWave.DBModels;

namespace WorkWave.Services.Abstracts
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<List<string>> GetAllRolesAsync();
       /* Task<List<User>> GetUsersInRoleAsync(string roleName);*/
    }
}
