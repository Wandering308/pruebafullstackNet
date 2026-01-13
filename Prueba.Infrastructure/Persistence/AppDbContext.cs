using Microsoft.EntityFrameworkCore;
using Prueba.Domain.Entities;
using Prueba.Domain.ValueObjects;

namespace Prueba.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Order
        var order = modelBuilder.Entity<Order>();
        order.HasKey(x => x.Id);

        order.Property(x => x.Customer).IsRequired().HasMaxLength(200);
        order.Property(x => x.Product).IsRequired().HasMaxLength(200);
        order.Property(x => x.Quantity).IsRequired();

        order.Property(x => x.DistanceKm).IsRequired();
        order.Property(x => x.CostUsd).IsRequired();

        // Value Objects como Owned (GeoPoint)
        order.OwnsOne(x => x.Origin, b =>
        {
            b.Property(p => p.Latitude).HasColumnName("OriginLat").IsRequired();
            b.Property(p => p.Longitude).HasColumnName("OriginLon").IsRequired();
        });

        order.OwnsOne(x => x.Destination, b =>
        {
            b.Property(p => p.Latitude).HasColumnName("DestinationLat").IsRequired();
            b.Property(p => p.Longitude).HasColumnName("DestinationLon").IsRequired();
        });
    }
}
