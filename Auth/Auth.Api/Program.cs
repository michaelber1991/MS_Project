using Auth.Api.Extensions;
using Auth.Application.Extensions;
using Auth.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseApiConfiguration(app.Environment);
app.MapControllers();
app.Run();