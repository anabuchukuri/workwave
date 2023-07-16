namespace WorkWave.DBModels
{
    public class JobType
    {
        public int JobTypeId { get; set; }

        public string Name { get; set; }

        public List<JobOpening> JobOpenings { get; set; }
    }
}
