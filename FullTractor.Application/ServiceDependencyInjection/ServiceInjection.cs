using FullTractor.Application.DTOs.User.Request;
using FullTractor.Application.Interfaces;
using FullTractor.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FullTractor.Application.ServiceDependencyInjection;

public static class ServiceInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Add DI of passwordhasher
        services.AddTransient<IPasswordHasher<UserRequest>, PasswordHasher<UserRequest>>();
        // Add DI of service dependencies
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}