using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WorkWave.DBModels
{
    public class User: IdentityUser<int>
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Role { get; set; }

        public int? EmployerId { get; set; }
        public int? JobSeekerId { get; set; }

        public Employer? EmployerProfile { get; set; }

        public JobSeeker? JobSeekerProfile { get; set; }
    }
}
