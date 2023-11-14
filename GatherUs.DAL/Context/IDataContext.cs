using GatherUs.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GatherUs.DAL.Context;

public interface IDataContext : IDisposable
{
    public DbSet<Guest> Guests { get; set; }
    
    public DbSet<Organizer> Organizers { get; set; }
    
    DatabaseFacade Database { get; }

    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    EntityEntry Entry(object entity);

    Task<int> DefaultEFSaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    ChangeTracker ChangeTracker { get; }
}