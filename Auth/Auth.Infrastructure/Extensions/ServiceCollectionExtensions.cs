using Auth.Application.Interfaces;
using Auth.Application.Interfaces.Repositories;
using Auth.Infrastructure.Persistence.Context;
using Auth.Infrastructure.Persistence.Repositories;
using Auth.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DbConnection")));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}