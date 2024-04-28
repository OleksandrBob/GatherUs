using GatherUs.DAL.Models;
using GatherUs.Enums.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace GatherUs.DAL.Context;

public class DataContext : DbContext, IDataContext
{
    private readonly IConnectionStrings _connectionStrings;

    public DataContext() { }

    public DataContext(IConnectionStrings connectionStrings)
    {
        _connectionStrings = connectionStrings;
    }

    public DbSet<Guest> Guests { get; set; }

    public DbSet<Organizer> Organizers { get; set; }
    
    public DbSet<CustomEvent> CustomEvents { get; set; }
    
    public DbSet<AttendanceInvite> AttendanceInvites { get; set; }
    
    public DbSet<EmailForRegistration> EmailForRegistrations { get; set; }
    
    public DbSet<GatherUsPaymentTransaction> GatherUsPaymentTransactions { get; set; }
    
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

        modelBuilder.Entity<User>()
            .HasDiscriminator(x => x.UserType)
            .HasValue<Organizer>(UserType.Organizer)
            .HasValue<Guest>(UserType.Guest);
        modelBuilder.Entity<User>().Property(u => u.UserType).HasDefaultValue(UserType.Guest);

        modelBuilder.Entity<EmailForRegistration>()
            .HasAlternateKey(e => e.Email);
            
        modelBuilder.Entity<CustomEvent>()
            .HasOne(e => e.Organizer)
            .WithMany(e => e.CreatedEvents)
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CustomEvent>()
            .HasMany(e => e.Attendants)
            .WithMany(e => e.PaidEvents)
            .UsingEntity<CustomEventGuest>(
                joinEntity =>
                {
                    joinEntity.HasOne(customEventGuest => customEventGuest.CustomEvent)
                        .WithMany(customEvent => customEvent.CustomEventGuests)
                        .HasForeignKey(customEventGuest => customEventGuest.CustomEventId)
                        .OnDelete(DeleteBehavior.Restrict);

                    joinEntity.HasOne(customEventGuest => customEventGuest.Guest)
                        .WithMany(guest => guest.CustomEventGuests)
                        .HasForeignKey(customEventGuest => customEventGuest.GuestId)
                        .OnDelete(DeleteBehavior.Restrict);

                    joinEntity.HasKey(x => new { x.GuestId, x.CustomEventId });
                });
        
        modelBuilder.Entity<AttendanceInvite>()
            .HasOne(e => e.CustomEvent)
            .WithMany(e => e.Invites)
            .HasForeignKey(e => e.CustomEventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AttendanceInvite>()
            .HasOne(e => e.Guest)
            .WithMany(e => e.Invites)
            .HasForeignKey(e => e.GuestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GatherUsPaymentTransaction>()
            .HasOne(e => e.Organizer)
            .WithOne();

        modelBuilder.Entity<GatherUsPaymentTransaction>()
            .HasOne(e => e.Guest)
            .WithOne();
        
        modelBuilder.Entity<GatherUsPaymentTransaction>()
            .HasOne(e => e.CustomEvent)
            .WithOne();
    }
}
