namespace Server.Csharp.Business.Models.Responses
{
    public class SessionResponse
    {
        public string AccessToken { get; set; }
        public Guid UserId { get; set; }
    }
}
