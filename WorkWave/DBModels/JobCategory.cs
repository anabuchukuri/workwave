namespace WorkWave.DBModels
{
    public class JobCategory
    {
        public int JobCategoryId { get; set; }

        public string Name { get; set; }

        public List<OpeningCategory> OpeningCategories { get; set; }
    }
}
