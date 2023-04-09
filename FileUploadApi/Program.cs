
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// Demo upload file
app.MapPost("/upload", async (IFormFile file) =>
{
    //Do something with the file
    using var stream = File.OpenWrite(file.FileName);
    await file.CopyToAsync(stream);

    return Results.Ok(file.FileName);
});

// Demo upload several files
app.MapPost("/uploadMany", async (IFormFileCollection files) =>
{
    var filenames = new string[] { };

    foreach (var file in files)
    {
        //Do something with the file
        using var stream = File.OpenWrite(file.FileName);
        await file.CopyToAsync(stream);

        filenames.Append(file.FileName);
    }
    return Results.Ok(filenames);
});

app.Run();