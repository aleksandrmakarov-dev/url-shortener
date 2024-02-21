namespace Server.API.Common
{
    public static class Constants
    {
        public const string UserContextName = "user";
        public static CookieOptions CookieOptions(DateTime? expires = null) => new CookieOptions
        {
            HttpOnly = true,
            Expires = expires,
        };
    }
}
