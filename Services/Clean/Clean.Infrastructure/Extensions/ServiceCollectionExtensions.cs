using Clean.Application.Interfaces;
using Clean.Application.Interfaces.Repositories;
using Clean.Infrastructure.Common.Handlers;
using Clean.Infrastructure.Persistence.Context;
using Clean.Infrastructure.Persistence.Repositories;
using Clean.Infrastructure.Persistence.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Clean.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCorsConfiguration();
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddScoped(typeof(INotificationHandler<>), typeof(SignalREventHandler<>));
    }

    private static void AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var connectionString = configuration.GetConnectionString("MongoConnection");
            return new MongoClient(connectionString);
        });
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<CleanContext>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var configuration = sp.GetRequiredService<IConfiguration>();
            var databaseName = configuration.GetSection("MongoSettings:Database").Value;
            return new CleanContext(mongoClient, databaseName!);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}