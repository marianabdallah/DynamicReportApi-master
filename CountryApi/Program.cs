var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/country_s/getall", () =>
{
    return new List<Country>
    {
        new Country
        {
            Id = 1,
            Name = "Pakistan"
        },
        new Country
        {
            Id = 2,
            Name = "India"
        },
        new Country
        {
            Id = 3,
            Name = "United States"
        }
    };
})
.WithName("GetAllCountries");

app.Run();

internal class Country
{
    public long Id { get; set; }
    public string Name { get; set; }
}
// test change