using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkWave.DBModels
{
    public class Employer
    {
        public int EmployerId { get; set; }

        public string CompanyName { get; set; }
        public string ContactNumber { get; set; }

        public string? Website { get; set; }
        public string? Address { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public List<JobOpening> JobOpenings { get; set; }
    }
}
