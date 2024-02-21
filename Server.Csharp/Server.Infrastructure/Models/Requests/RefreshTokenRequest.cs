using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Models.Requests;

public class RefreshTokenRequest
{
    [Required]
    public required string Token { get; set; }
}