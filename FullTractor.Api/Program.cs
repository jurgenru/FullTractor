using System.Text;
using System.Text.Json.Serialization;
using FullTractor.Api.Exceptions;
using FullTractor.Application.ServiceDependencyInjection;
using FullTractor.Infrastructure.Entities;
using FullTractor.Infrastructure.RepositoryDependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
//CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

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
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Add ProblemDetails forms.
builder.Services.AddProblemDetails();
//Control global generic exception
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
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

//CORS
app.UseCors();

//JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
