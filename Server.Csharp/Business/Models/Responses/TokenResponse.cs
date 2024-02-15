namespace Server.Csharp.Business.Models.Responses;

public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public Guid UserId { get; set; }
}