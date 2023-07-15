using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using WorkWave.DBModels;

namespace WorkWave.DBModels
{
    public class JobOpening
    {
        public int JobOpeningId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int EmployerId { get; set; }
        public  Employer Employer { get; set; }

        public JobType JobType { get; set; }

        public string JobTypeId { get; set; }

        public int JobDetailsId { get; set; }

        public JobDetails JobDetails { get; set; }

        public List<OpeningCategory> OpeningCategories { get; set; }
        public List<JobApplication> JobApplications { get; set; }
    }
}



