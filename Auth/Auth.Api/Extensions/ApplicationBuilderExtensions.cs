namespace Auth.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseSwaggerConfiguration();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();

        return app;
    }

    private static void UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Api");
            options.RoutePrefix = "swagger";
        });
    }
}