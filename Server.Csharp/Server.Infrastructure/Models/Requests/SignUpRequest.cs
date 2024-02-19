﻿using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Models.Requests
{
    public class SignUpRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
    }
}
