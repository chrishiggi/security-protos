public class CustomHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public CustomHeaderMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("X-Custom-Header", "Custom Header Text");
        await _next(context);
    }
}
