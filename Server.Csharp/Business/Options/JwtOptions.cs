namespace Server.Csharp.Business.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
    }
}
