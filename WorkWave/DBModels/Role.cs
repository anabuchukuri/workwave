using Microsoft.AspNetCore.Identity;

namespace WorkWave.DBModels
{
    public class Role : IdentityRole<int>
    {
        public int RoleId { get; set; }
    }
}