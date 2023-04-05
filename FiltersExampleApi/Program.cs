using FiltersExampleApi.EndpointFilters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Configures ApiExplorer using Metadata
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapGet("/BasicFilter/{firstname}", (string firstname) =>  $"Hello {firstname}!")
   .AddEndpointFilter(async (efiContext, next) =>
    {
        var firstname = efiContext.HttpContext.GetRouteData().Values["firstname"];
        var logger = efiContext.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Hello");
        logger.LogInformation($"AddEndpointFilter before filter using parameter firstname: {firstname}");
        var result = await next(efiContext);
        logger.LogInformation($"AddEndpointFilter after filter using parameter firstname: {firstname}");
        return result;
    });

app.MapGet("/CustomFilter/{firstname}", (string firstname) => $"Hello {firstname}!")
    .AddEndpointFilter<MyEndpointFilter>();

app.Run();
