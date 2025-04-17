using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Ocelot.DependencyInjection;

namespace MS_Project.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddCorsConfiguration();
        services.AddAuthenticationSchemes(configuration);
        services.AddOcelot(configuration);


        return services;
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

    private static IServiceCollection AddAuthenticationSchemes(this IServiceCollection services,
        IConfiguration configuration)
    {
        var certificateConfig = LoadCertificateConfig();

        services.AddAuthentication(options => { options.DefaultScheme = "CustomScheme"; })
            .AddPolicyScheme("CustomScheme", "Bearer or Certificate", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    if (context.Request.Headers.ContainsKey("Authorization"))
                        return "Bearer";

                    if (context.Connection.ClientCertificate != null)
                        return "Certificate";

                    return "None";
                };
            })
            .AddJwtBearer("Bearer", options =>
            {
                var jwtSettings = configuration.GetSection("Jwt");
                var issuer = jwtSettings["Issuer"];
                var secretKey = jwtSettings["SecretKey"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                };
            })
            .AddCertificate("Certificate", options =>
            {
                options.AllowedCertificateTypes = CertificateTypes.All;
                options.ValidateCertificateUse = true;
                options.RevocationMode = X509RevocationMode.Online;
                options.Events = new CertificateAuthenticationEvents
                {
                    OnCertificateValidated = context =>
                    {
                        var requestPath = context.HttpContext.Request.Path.Value;


                        if (certificateConfig != null)
                        {
                            var validCertsForPath = certificateConfig
                                .Where(c => c.AcceptedPaths != null && requestPath != null &&
                                            c.AcceptedPaths.Contains(requestPath))
                                .ToList();

                            var isValidCert = false;

                            foreach (var certConfig in validCertsForPath)
                            {
                                var clientCert = context.ClientCertificate;

                                if (clientCert.Subject.Contains(certConfig.Name) &&
                                    clientCert.Thumbprint == certConfig.Thumbprint)
                                {
                                    isValidCert = true;
                                    break;
                                }
                            }

                            if (isValidCert)
                                context.Success();
                            else
                                context.Fail("Certificado no válido para este endpoint.");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IConfigurationBuilder AddGatewayConfiguration(this IConfigurationBuilder config)
    {
        var environmentConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        config.AddJsonFile("ocelot.json", false, true);

        var routeFiles = Directory.GetFiles("routes", "*.json");
        foreach (var file in routeFiles)
        {
            var jsonContent = File.ReadAllText(file);
            jsonContent = ReplacePlaceholdersWithConfigValues(jsonContent, environmentConfig);
            File.WriteAllText(file, jsonContent);
            config.AddJsonFile(file, true, true);
        }

        return config;
    }

    private static string ReplacePlaceholdersWithConfigValues(string jsonContent, IConfiguration config)
    {
        foreach (var key in config.AsEnumerable())
        {
            var placeholder = $"{{{{{key.Key}}}}}";
            if (jsonContent.Contains(placeholder)) jsonContent = jsonContent.Replace(placeholder, key.Value);
        }

        return jsonContent;
    }


    private static List<CertificateConfig>? LoadCertificateConfig()
    {
        var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Configurations",
            "certificate-configuration.json");
        var json = File.ReadAllText(configFilePath);
        var config = JObject.Parse(json);

        var certificates = config["certificates"]?.ToObject<List<CertificateConfig>>();
        return certificates;
    }
}

internal class CertificateConfig
{
    public required string Name { get; set; }
    public required string Thumbprint { get; set; }
    public List<string>? AcceptedPaths { get; set; }
}