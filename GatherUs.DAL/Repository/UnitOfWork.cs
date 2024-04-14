using CSharpFunctionalExtensions;
using GatherUs.DAL.Context;
using GatherUs.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GatherUs.DAL.Repository;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private bool _disposed;
    private const int MaxSaveDataLoops = 10;
    private readonly IDataContext _context;

    private IDataRepository<User> _users;
    private IDataRepository<Guest> _guests;
    private IDataRepository<Organizer> _organizers;
    private IDataRepository<CustomEvent> _customEvents;
    private IDataRepository<AttendanceInvite> _attendanceInvites;
    private IDataRepository<EmailForRegistration> _emailForRegistrations;
    private IDataRepository<GatherUsPaymentTransaction> _gatherUsPaymentTransactios;

    public IDataRepository<User> Users => _users ??= new DataRepository<User>(_context);

    public IDataRepository<Guest> Guests => _guests ??= new DataRepository<Guest>(_context);

    public IDataRepository<Organizer> Organizers => _organizers ??= new DataRepository<Organizer>(_context);

    public IDataRepository<CustomEvent> CustomEvents => _customEvents ??= new DataRepository<CustomEvent>(_context);

    public IDataRepository<AttendanceInvite> AttendanceInvites =>
        _attendanceInvites ??= new DataRepository<AttendanceInvite>(_context);

    public IDataRepository<EmailForRegistration> EmailForRegistrations =>
        _emailForRegistrations ??= new DataRepository<EmailForRegistration>(_context);

    public IDataRepository<GatherUsPaymentTransaction> GatherUsPaymentTransactions => _gatherUsPaymentTransactios ??=
        new DataRepository<GatherUsPaymentTransaction>(_context);

    public UnitOfWork(IDataContext context)
    {
        _context = context;
    }

    public async Task PerformTransactionAsync(List<Func<Task<Result>>> transactionParts)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach (var part in transactionParts)
            {
                var res = await part.Invoke();

                if (res.IsFailure)
                {
                    await transaction.RollbackAsync();
                    return;
                }
            }
        }
        catch
        {
            await transaction.RollbackAsync();
        }

        await transaction.CommitAsync();
    }

    public void Complete(bool withTimeUpdate = true)
    {
        if (withTimeUpdate)
        {
            _context.SaveChanges();
        }
        else
        {
            _context.DefaultEFSaveChangesAsync();
        }
    }

    public async Task CompleteAsync(bool withTimeUpdate = true)
    {
        var savedData = false;
        var loopCounter = 0;
        while (!savedData && loopCounter <= MaxSaveDataLoops)
        {
            try
            {
                loopCounter++;

                if (withTimeUpdate)
                {
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await _context.DefaultEFSaveChangesAsync();
                }

                savedData = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is RefreshToken)
                    {
                        var currentValues = entry.CurrentValues;
                        var originalValue = entry.GetDatabaseValues();

                        // If concurrency problem caused by deleting
                        if (originalValue is null)
                        {
                            // Reload entry
                            await entry.ReloadAsync();
                            continue;
                        }

                        foreach (var property in entry.Metadata.GetProperties())
                        {
                            // TODO: Logic to decide which value should be written to database
                            // Currently the same optimistic resolution as default
                        }

                        entry.OriginalValues.SetValues(currentValues);
                    }
                }
            }
        }
    }

    public void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
