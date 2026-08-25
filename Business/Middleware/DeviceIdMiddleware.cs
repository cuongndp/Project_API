namespace Business.Middleware
{
    public class DeviceIdMiddleware
    {
        private readonly RequestDelegate _next;

        public DeviceIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            const string cookieName = "DeviceID";

            if (!context.Request.Cookies.TryGetValue(cookieName, out var deviceId)
                || string.IsNullOrEmpty(deviceId))
            {
                deviceId = Guid.NewGuid().ToString();

                context.Response.Cookies.Append(cookieName, deviceId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });
            }
            context.Items["DeviceID"] = deviceId;
            await _next(context);
        }
    }
}
