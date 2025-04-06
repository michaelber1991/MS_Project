using Clean.Application.Interfaces;
using Clean.Application.Interfaces.Repositories;
using Clean.Infrastructure.Persistence.Context;
using Clean.Infrastructure.Persistence.Repositories;
using Clean.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CleanContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DbConnection")));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}