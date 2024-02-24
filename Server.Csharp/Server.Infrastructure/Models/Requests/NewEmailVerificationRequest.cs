﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Requests 
{
    public class NewEmailVerificationRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
