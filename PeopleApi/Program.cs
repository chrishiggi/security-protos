var builder = WebApplication.CreateBuilder(args);

List<PersonModel> people = new()
{
    new() { Id = 1, Name = "Moe" },
    new() { Id = 2, Name = "Larry" },
    new() { Id = 3, Name = "Curly" },
};

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/People", () => people);
app.MapPost("/People", (PersonModel p) => people.Add(p));
app.MapPut("/People/{id}", (int id, PersonModel p) =>
{
    var person = people.Where(x => x.Id == id).FirstOrDefault();
    if (person is not null)
    {
        person.Name = p.Name;
    }

    return person;
});
app.MapDelete("/People/{id}", (int id) =>
{
    var p = people.Find(x => x.Id == id);

    if (p is not null)
    {
        people.Remove(p);
    }
});

app.Run();
