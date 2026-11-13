using FullTractor.Application.Interfaces;
using FullTractor.Domain.Interfaces;
using FullTractor.Infrastructure.Auth;
using FullTractor.Infrastructure.Context;
using FullTractor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FullTractor.Infrastructure.RepositoryDependencyInjection;

public static class RepositoryInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //DbContext connection string
        string connectionString = configuration.GetConnectionString("DbCon") ?? throw new InvalidOperationException("Connection string 'DbCon' not found.");
        // Add context to the project.
        services.AddDbContext<FullTractorContext>(options => options.UseSqlServer(connectionString));
        // Add DI of repository dependencies
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        // Add DI of token depencies
        services.AddSingleton<ITokenService, TokenService>();
        return services;
    }
}