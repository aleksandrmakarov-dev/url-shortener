namespace Server.Csharp.Business.Models.Common
{
    public class Token
    {
        public string Value { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
