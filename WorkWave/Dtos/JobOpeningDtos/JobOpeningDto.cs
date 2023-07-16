using WorkWave.DBModels;

namespace WorkWave.Dtos.JobOpeningDtos
{
    public class JobOpeningDto
    {
        public int JobOpeningId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
