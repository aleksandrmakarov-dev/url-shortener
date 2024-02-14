using System.ComponentModel.DataAnnotations;

namespace Server.Csharp.Business.Models.Requests;

public class SignInRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(5)]
    public string Password { get; set; }
}