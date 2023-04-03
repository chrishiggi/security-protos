using System.Text;

public class BasicAuthHandler
{
    private readonly RequestDelegate next;
    private readonly string relm;

    public BasicAuthHandler(RequestDelegate next, string relm)
    {
        this.next = next;
        this.relm = relm;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized...");
            return;
        }

        // Basic userId:password
        var header = context.Request.Headers["Authorization"].ToString();
        var encodedCreds = header.Substring(6);
        var creds = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCreds));
        string[] uidpw = creds.Split(":");
        var uid = uidpw[0];
        var pw = uidpw[1];

        if (uid != "MyUser" && pw != "MyPassword")
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized...");
            return;
        }

        await next(context);
    }
}