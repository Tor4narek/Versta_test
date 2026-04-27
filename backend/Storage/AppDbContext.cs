using Microsoft.EntityFrameworkCore;
using Storage.Entities;

namespace Storage;

/// <summary>
/// Контекст базы данных приложения.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Заказы.
    /// </summary>
    public DbSet<Order> Orders => Set<Order>();

    /// <summary>
    /// Создает экземпляр <see cref="AppDbContext"/>.
    /// </summary>
    /// <param name="options">Параметры конфигурации контекста базы данных.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    /// <summary>
    /// Настраивает модель данных приложения.
    /// </summary>
    /// <param name="modelBuilder">Объект для настройки сущностей и связей.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureOrder(modelBuilder);
    }

    /// <summary>
    /// Настраивает отображение сущности <see cref="Order"/> в таблицу базы данных.
    /// </summary>
    /// <param name="modelBuilder">Объект для настройки сущностей.</param>
    private static void ConfigureOrder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.OrderNumber)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.HasIndex(x => x.OrderNumber)
                .IsUnique();

            entity.Property(x => x.CitySender)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.AddressSender)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(x => x.CityReceiver)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.AddressReceiver)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(x => x.Weight)
                .IsRequired()
                .HasColumnType("numeric(10,3)");

            entity.Property(x => x.PickUpDate)
                .IsRequired();
        });
    }
}