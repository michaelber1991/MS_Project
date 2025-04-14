using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddAuthenticationSchemes(configuration);

        return services;
    }

    private static IServiceCollection AddAuthenticationSchemes(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication()
            // .AddSaml2("Saml2", options =>
            // {
            //     options.SPOptions.EntityId = new EntityId("https://tu-api.com/Saml2");
            //     options.IdentityProviders.Add(new IdentityProvider(
            //         new EntityId("https://pingfederate.com/idp"), options.SPOptions)
            //     {
            //         LoadMetadata = true,
            //         MetadataLocation = "https://pingfederate.com/idp/metadata"
            //     });
            // })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "tu-emisor",
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("una-clave-secreta-mas-larga-de-256-bits"))
                };
            });
        return services;
    }
}