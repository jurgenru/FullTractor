using FullTractor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DbCon") ?? throw new InvalidOperationException("Connection string 'DbCon' not found.");

// Add services to the container.
builder.Services.AddControllers();
// Add context to the project.
builder.Services.AddDbContext<FullTractorContext>(options => options.UseSqlServer(connectionString));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
