namespace WorkWave.Dtos.JobDetailDtos
{
    public class JobDetailAddDto
    {
        public string? EmploymentType { get; set; }

        public DateTime? ApplicationDeadline { get; set; }

        public int? RequiredExperience { get; set; }

        public string? Qualifications { get; set; }

        public string? Responsibilities { get; set; }

        public string? CompanyCulture { get; set; }

        public string? ApplicationInstructions { get; set; }

        public int? NumberOfOpenings { get; set; }

        public bool? IsFullTime { get; set; }

        public bool? IsRemote { get; set; }
    }
}
