using WorkWave.DBModels;

namespace WorkWave.Dtos.UserDtos
{
    public class JobSeekerRegistrationDto : UserRegistrationDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? ResumeUrl { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? GithubProfile { get; set; }
        public string? Skills { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public DateTime? DateOfBirth { get; set; }


    }
}
