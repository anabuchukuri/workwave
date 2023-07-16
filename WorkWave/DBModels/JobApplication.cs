using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkWave.DBModels
{
    public class JobApplication
    {
        public int ApplicationId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string CoverLetter { get; set; }
        public int JobSeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }
        public int JobOpeningId { get; set; }
        public JobOpening JobOpening { get; set; }
    }
}
