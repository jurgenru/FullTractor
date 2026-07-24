using System.Text;
using FullTractor.Api.Exceptions;
using FullTractor.Application.DTOs.User.Request;
using FullTractor.Application.Interfaces;
using FullTractor.Application.Services;
using FullTractor.Domain.Interfaces;
using FullTractor.Infrastructure.Auth;
using FullTractor.Infrastructure.Context;
using FullTractor.Infrastructure.Entities;
using FullTractor.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
//DbContext connection string
string connectionString = builder.Configuration.GetConnectionString("DbCon") ?? throw new InvalidOperationException("Connection string 'DbCon' not found.");
//Appsettings.json get section JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddAuthentication().AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:Key").Get<string>()!)),
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration.GetSection("JwtSettings:Issuer").Get<string>(),
    ValidateAudience = true,
    ValidAudience = builder.Configuration.GetSection("JwtSettings:Audience").Get<string>(),
    ValidateLifetime = true,
});

// Add services to the container.
builder.Services.AddControllers();
// Add ProblemDetails forma.
builder.Services.AddProblemDetails();
//Control global generic exception
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// Add context to the project.
builder.Services.AddDbContext<FullTractorContext>(options => options.UseSqlServer(connectionString));
//Add DI of passwordhasher
builder.Services.AddTransient<IPasswordHasher<UserRequest>, PasswordHasher<UserRequest>>();
// Add DI of repository dependencies
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Add DI of service dependencies
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
// Add DI of token depencies
builder.Services.AddSingleton<ITokenService, TokenService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
//Catch global exceptions and translate it to ProblemDetails format
app.UseExceptionHandler();
//Catch from 400 to 599 errors and translate it to ProblemDetails format
app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

//JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
