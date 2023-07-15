using System.ComponentModel.DataAnnotations;

namespace WorkWave.DBModels
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Role { get; set; }

        public Employer EmployerProfile { get; set; }

        public JobSeeker JobSeekerProfile { get; set; }
    }
}
