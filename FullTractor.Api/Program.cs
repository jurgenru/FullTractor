using FullTractor.Application.DTOs;
using FullTractor.Application.Interfaces;
using FullTractor.Application.Services;
using FullTractor.Domain.Interfaces;
using FullTractor.Infrastructure.Context;
using FullTractor.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DbCon") ?? throw new InvalidOperationException("Connection string 'DbCon' not found.");

// Add services to the container.
builder.Services.AddControllers();
// Add ProblemDetails forma.
builder.Services.AddProblemDetails();
// Add context to the project.
builder.Services.AddDbContext<FullTractorContext>(options => options.UseSqlServer(connectionString));
//Add DI of passwordhasher
builder.Services.AddScoped<IPasswordHasher<UserRequest>, PasswordHasher<UserRequest>>();
// Add DI of repository depedencies
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// Add DI of service depedencies
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//Catch global exceptions and translate it to ProblemDetails format
app.UseExceptionHandler();
//Cathc from 400 to 599 errors and translate it to ProblemDetails format
app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
