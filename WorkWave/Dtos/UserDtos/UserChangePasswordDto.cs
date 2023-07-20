namespace WorkWave.Dtos.UserDtos
{
    public class UserChangePasswordDto
    {
        public string Username { get; set; }

        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
