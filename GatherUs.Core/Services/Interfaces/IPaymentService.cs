namespace GatherUs.Core.Services.Interfaces;

public interface IPaymentService
{
    string GenerateClientToken(int userId);

    Task ProcessPayment(string nonce);
}