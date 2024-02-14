using System.ComponentModel.DataAnnotations;

namespace Server.Csharp.Business.Models.Requests
{
    public class VerifyEmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
