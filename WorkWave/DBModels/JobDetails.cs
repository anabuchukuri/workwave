namespace WorkWave.DBModels
{
    public class JobDetails
    {
    public int JobDetailsId { get; set; }

    public string EmploymentType { get; set; }

    public DateTime ApplicationDeadline { get; set; }

    public int RequiredExperience { get; set; }

    public string Qualifications { get; set; }

    public string Responsibilities { get; set; }

    public string CompanyCulture { get; set; }

    public string ApplicationInstructions { get; set; }

    public int NumberOfOpenings { get; set; }

    public Boolean IsFullTime { get; set; }

    public Boolean IsRemote { get; set; }

    public int JobOpeningId { get; set; }
    public JobOpening JobOpening { get; set; }
    }
}
