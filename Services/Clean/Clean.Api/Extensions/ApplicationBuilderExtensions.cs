using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.IdentityModel.Tokens;

namespace Clean.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors();
        app.Use(async (context, next) =>
        {
            
            if (context.Request.Path.StartsWithSegments("/hangfire") &&
                !context.Request.Headers.ContainsKey("Authorization"))
            {
                var token = context.Request.Cookies["access_token"];

                if (!string.IsNullOrEmpty(token)) context.Request.Headers["Authorization"] = $"Bearer {token}";
            }

            await next();
        });

        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        });
        app.UseHttpsRedirection();

        return app;
    }
}

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
            return false;

        var config = httpContext.RequestServices.GetRequiredService<IConfiguration>();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}