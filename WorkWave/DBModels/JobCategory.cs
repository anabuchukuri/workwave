namespace WorkWave.DBModels
{
    public class JobCategory
    {
        public string JobCategoryId { get; set; }

        public string Name { get; set; }

        public List<Job> Job { get; set; }
    }
}
