using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Storage;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

Env.Load(Path.Combine(builder.Environment.ContentRootPath, "..", ".env"));

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
            .AllowAnyOrigin()
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

app.UseHttpsRedirection();

app.MapControllers();

app.Run();