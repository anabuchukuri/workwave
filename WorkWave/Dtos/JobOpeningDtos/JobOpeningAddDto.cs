﻿using WorkWave.DBModels;

namespace WorkWave.Dtos.JobOpeningDtos
{
    public class JobOpeningAddDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public int? JobTypeId { get; set; }
        public List<int>? JobCategoryList { get; set; }

        public string? EmploymentType { get; set; }

        public DateTime? ApplicationDeadline { get; set; }

        public int? RequiredExperience { get; set; }

        public string? Qualifications { get; set; }

        public string? Responsibilities { get; set; }

        public string? CompanyCulture { get; set; }

        public string? ApplicationInstructions { get; set; }

        public int? NumberOfOpenings { get; set; }

        public Boolean? IsFullTime { get; set; }

        public Boolean? IsRemote { get; set; }

    }
}
