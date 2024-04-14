using CSharpFunctionalExtensions;
using GatherUs.DAL.Models;

namespace GatherUs.DAL.Repository;

public interface IUnitOfWork
{
    public IDataRepository<User> Users { get; }

    public IDataRepository<Guest> Guests { get; }

    public IDataRepository<Organizer> Organizers { get; }

    public IDataRepository<CustomEvent> CustomEvents { get; }

    public IDataRepository<AttendanceInvite> AttendanceInvites { get; }

    public IDataRepository<EmailForRegistration> EmailForRegistrations { get; }

    public IDataRepository<GatherUsPaymentTransaction> GatherUsPaymentTransactions { get; }

    Task PerformTransactionAsync(List<Func<Task<Result>>> funcs);

    public void Complete(bool withTimeUpdate = true);

    public Task CompleteAsync(bool withTimeUpdate = true);
}
