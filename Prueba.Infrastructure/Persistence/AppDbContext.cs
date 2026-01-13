using Microsoft.EntityFrameworkCore;
using Prueba.Domain.Entities;

namespace Prueba.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var b = modelBuilder.Entity<Order>();

        b.ToTable("Orders");
        b.HasKey(x => x.Id);

        b.Property(x => x.Customer).HasMaxLength(200).IsRequired();
        b.Property(x => x.Product).HasMaxLength(200).IsRequired();
        b.Property(x => x.Quantity).IsRequired();

        // GeoPoint (Origin)
        b.OwnsOne(x => x.Origin, owned =>
        {
            owned.Property(o => o.Latitude).HasColumnName("OriginLat");
            owned.Property(o => o.Longitude).HasColumnName("OriginLon");
        });

        // GeoPoint (Destination)
        b.OwnsOne(x => x.Destination, owned =>
        {
            owned.Property(o => o.Latitude).HasColumnName("DestinationLat");
            owned.Property(o => o.Longitude).HasColumnName("DestinationLon");
        });

        b.Property(x => x.DistanceKm).IsRequired();
        b.Property(x => x.CostUsd).IsRequired();
    }
}
