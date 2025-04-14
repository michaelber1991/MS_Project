using Ocelot.Middleware;

namespace MS_Project.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task UseCustomGatewayAsync(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        await app.UseOcelot();
    }
}