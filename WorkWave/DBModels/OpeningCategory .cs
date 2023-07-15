using System.ComponentModel.DataAnnotations.Schema;

namespace WorkWave.DBModels
{
    public class OpeningCategory
    {
        public int JobOpeningId { get; set; }
        public int JobCategoryId { get; set; }
        public JobOpening JobOpening { get; set; }
        public JobCategory JobCategory { get; set; }
    }
}
