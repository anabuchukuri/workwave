using WorkWave.DBModels;

namespace WorkWave.Dtos.UserDtos
{
    public class EmployerRegistrationDto : UserRegistrationDto
    {
        public string CompanyName { get; set; }
        public string ContactNumber { get; set; }

        public string? Address { get; set; }
        public string? Website { get; set; }


    }
}
