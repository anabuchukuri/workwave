namespace WorkWave.Dtos.JobApplicationDtos
{
    public class JobApplicationGetDto
    {
        public int ApplicationId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string? CoverLetter { get; set; }
        public string? References { get; set; }
        public string? JobSpecificCV { get; set; }
        public int JobSeekerId { get; set; }
        public string JobSeekerName { get; set; }
        public int JobOpeningId { get; set; }
        public string JobOpeningCompanyName { get; set; }
    }
}
