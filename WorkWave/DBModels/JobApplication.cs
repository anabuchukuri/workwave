using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WorkWave.Constants;

namespace WorkWave.DBModels
{
    public class JobApplication
    {
        public int ApplicationId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string? CoverLetter { get; set; }
        public string? References { get; set; }
        public string? JobSpecificCV { get; set; }
        public int JobSeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }
        public int JobOpeningId { get; set; }
        public JobOpening JobOpening { get; set; }

        public Status Status { get;set; } = Status.Pending;
    }
}
