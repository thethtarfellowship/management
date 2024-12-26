﻿namespace management.Models
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; } // Optional role
    }
}
