namespace Server.API.Common
{
    public static class Helpers
    {
        public static string? GetIpAddress(HttpContext httpContext)
        {
            var ipAddress = httpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") ?? 
                            httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

            return httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }
    }
}
