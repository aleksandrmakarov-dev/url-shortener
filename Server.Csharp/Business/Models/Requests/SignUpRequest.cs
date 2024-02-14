using System.ComponentModel.DataAnnotations;

namespace Server.Csharp.Business.Models.Requests
{
    public class SignUpRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
