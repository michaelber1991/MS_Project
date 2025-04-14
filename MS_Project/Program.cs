using MS_Project.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddGatewayConfiguration();
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

await app.UseCustomGatewayAsync();

app.Run();