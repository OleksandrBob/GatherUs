using GatherUs.DAL.Enums;
using GatherUs.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GatherUs.DAL.Context;

public class DataContext : DbContext, IDataContext
{
    private readonly IConnectionOptions _options;

    public DataContext(IConnectionOptions options)
    {
        _options = options;
    }

    public DbSet<Guest> Guests { get; set; }
    
    public DbSet<Organizer> Organizers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_options.ConnectionString)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Guest>().ToTable("Users");
        modelBuilder.Entity<Organizer>().ToTable("Users");
        modelBuilder.Entity<Admin>().ToTable("Users");

        // Soft delete global query filter
        modelBuilder.Entity<User>().HasQueryFilter(user => user.DeletionTime == null);

        modelBuilder.Entity<User>()
            .HasDiscriminator(x => x.UserType)
            .HasValue<Organizer>(UserType.Organizer)
            .HasValue<Guest>(UserType.Guest)
            .HasValue<Admin>(UserType.Admin);
    }
}