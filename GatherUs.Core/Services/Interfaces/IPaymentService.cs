using CSharpFunctionalExtensions;

namespace GatherUs.Core.Services.Interfaces;

public interface IPaymentService
{
    Task<Result> ReplenishBalance(int userId, string nonce, decimal amount);

    Task<string> CreateUser(string email, string firstName, string lastName);

    Task<string> GenerateClientToken(int userId);
}
