﻿using WorkWave.DBModels;

namespace WorkWave.Dtos.UserDtos
{
    public class UserLoginDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Boolean rememberme { get; set; }
    }
}