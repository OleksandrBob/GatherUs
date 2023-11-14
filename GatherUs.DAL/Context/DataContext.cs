using GatherUs.DAL.Models;
using GatherUs.Enums.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace GatherUs.DAL.Context;

public class DataContext : DbContext, IDataContext
{
    private readonly IConnectionStrings _connectionStrings;

    public DataContext()
    {
    }

    public DataContext(IConnectionStrings connectionStrings)
    {
        _connectionStrings = connectionStrings;
    }

    public DbSet<Guest> Guests { get; set; }

    public DbSet<Organizer> Organizers { get; set; }

    public async Task<int> DefaultEFSaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_connectionStrings?.ConnectionString != null)
        {
            optionsBuilder.UseNpgsql(_connectionStrings.ConnectionString);
        }
        else
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("C:\\un\\diploma\\GatherUs\\GatherUs.DAL\\dbsettings.json", false)
                .Build();

            optionsBuilder.UseNpgsql(configuration
                .GetSection("ConnectionOptions:ConnectionStringConfig")
                .Value);
        }
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
        modelBuilder.Entity<User>().Property(u => u.UserType).HasDefaultValue(UserType.Guest);

    }
}