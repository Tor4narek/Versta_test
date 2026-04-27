using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services;
using Services.Interfaces;
using Storage;

var builder = WebApplication.CreateBuilder(args);

Env.Load(Path.Combine(builder.Environment.ContentRootPath, "..", ".env"));

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = AppDbContextFactory.BuildPostgresConnectionString();
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Delivery API",
        Version = "v1",
        Description = "API для создания и просмотра заказов на доставку"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                {
                    return false;
                }

                return uri.Host == "localhost" || uri.Host == "127.0.0.1";
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery API v1");
    options.RoutePrefix = "swagger";
});

app.UseCors("Frontend");

app.MapControllers();

app.Run();

