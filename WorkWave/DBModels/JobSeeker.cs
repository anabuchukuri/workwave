using Microsoft.AspNetCore.Builder;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkWave.DBModels
{
    public class JobSeeker
    {
        public int JobSeekerId { get; set; }
        public string ResumeUrl { get; set; }
        public string LinkedInProfile { get; set; }
        public string GithubProfile { get; set; }
        public string Skills { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int UserId { get; set; }
        public  User User { get; set; }
        public  List<JobApplication> JobApplications { get; set; }

    }
}
