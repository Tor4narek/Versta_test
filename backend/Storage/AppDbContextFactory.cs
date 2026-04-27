using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Storage;

/// <summary>
/// Фабрика для создания AppDbContext во время выполнения design-time операций.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Создает экземпляр <see cref="AppDbContext"/> для design-time операций Entity Framework Core.
    /// </summary>
    /// <param name="args">Аргументы командной строки, переданные инструментами EF Core.</param>
    /// <returns>Настроенный экземпляр <see cref="AppDbContext"/>.</returns>
    public AppDbContext CreateDbContext(string[] args)
    {
        var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "");
        var envPath = Path.Combine(apiProjectPath, ".env");

        Env.Load(envPath);

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = BuildPostgresConnectionString();

        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
    
    /// <summary>
    /// Формирует строку подключения к PostgreSQL на основе переменных окружения.
    /// </summary>
    /// <returns>Строка подключения к базе данных PostgreSQL.</returns>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если одна из обязательных переменных окружения не задана.
    /// </exception>
    public static string BuildPostgresConnectionString()
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

        if (string.IsNullOrWhiteSpace(host))
            throw new InvalidOperationException("POSTGRES_HOST is not set.");

        if (string.IsNullOrWhiteSpace(port))
            throw new InvalidOperationException("POSTGRES_PORT is not set.");

        if (string.IsNullOrWhiteSpace(database))
            throw new InvalidOperationException("POSTGRES_DB is not set.");

        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidOperationException("POSTGRES_USER is not set.");

        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("POSTGRES_PASSWORD is not set.");

        return $"Host={host};Port={port};Database={database};Username={username};Password={password}";
    }
}