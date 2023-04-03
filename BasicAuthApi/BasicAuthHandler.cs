using System.Text;

public class BasicAuthHandler
{
    private const int UserIdIndex = 0;
    private const int PasswordIndex = 1;
    private const string Separator = ":";
    private const string UnauthorizedText = "Unauthorized...";
    private const string AuthorizationHeaderKey = "Authorization";
    private readonly RequestDelegate next;
    private readonly string relm;

    public BasicAuthHandler(RequestDelegate next, string relm)
    {
        this.next = next;
        this.relm = relm;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(AuthorizationHeaderKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(UnauthorizedText);
            return;
        }

        // Basic userId:password
        var header = context.Request.Headers[AuthorizationHeaderKey].ToString();
        var encodedCreds = header.Substring(6);
        var creds = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCreds));
        string[] uidpw = creds.Split(Separator);
        var uid = uidpw[UserIdIndex];
        var pw = uidpw[PasswordIndex];

        if (uid != "MyUser" || pw != "MyPassword")
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(UnauthorizedText);
            return;
        }

        await next(context);
    }
}