public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseConventionalMiddleware(
        this IApplicationBuilder app)
        => app.UseMiddleware<CustomHeaderMiddleware>();
}