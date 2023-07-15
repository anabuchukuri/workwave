namespace WorkWave.DBModels
{
    public class JobType
    {
        public string JobTypeId { get; set; }

        public string Name { get; set; }

        public List<JobOpening> JobOpenings { get; set; }
    }
}
