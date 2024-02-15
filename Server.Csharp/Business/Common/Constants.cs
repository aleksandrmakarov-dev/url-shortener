namespace Server.Csharp.Business.Common
{
    public static class Constants
    {
        public const string RefreshTokenCookie = "refresh-token";
        public const int RefreshTokenExpires = 10800;
        public const int JwtTokenExpires = 15;
        public const int EmailVerificationExpires = 720;
        public const string IdClaim = "id";
        public const string RoleClaim = "role";
        public const string User = "user-jwt-payload";
        public static CookieOptions CookieOptions(DateTime? expiresAt = null) => new CookieOptions
        {
            HttpOnly = true,
            Expires = expiresAt
        };
    }
}
