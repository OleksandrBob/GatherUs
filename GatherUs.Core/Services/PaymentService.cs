using GatherUs.Core.Services.Interfaces;
using CSharpFunctionalExtensions;
using GatherUs.Core.Mailing.SetUp;

namespace GatherUs.Core.Services;
using Braintree;

public class PaymentService : IPaymentService
{
    private readonly IUserService _userService;
    private readonly BraintreeGateway _apiGateway;

    public PaymentService(IUserService userService, IBraintreeOptions braintreeOptions)
    {
        _userService = userService;

        _apiGateway = new()
        {
            Environment = Environment.SANDBOX,
            MerchantId = braintreeOptions.MerchantId,
            PublicKey = braintreeOptions.PublicKey,
            PrivateKey = braintreeOptions.PrivateKey,
        };
    }

    public PaymentService(IUserService userService, BraintreeGateway gateway)
    {
        _userService = userService;
        _apiGateway = gateway;
    }

    public async Task<string> GenerateClientToken(int userId)
    {
        var user = await _userService.GetByIdAsync(userId);

        return await _apiGateway.ClientToken.GenerateAsync(new ClientTokenRequest
        {
            CustomerId = user.BrainTreeId,
        });
    }

    public async Task<Result> ReplenishBalance(int userId, string nonce, decimal amount)
    {
        var request = new TransactionRequest
        {
            Amount = amount,
            PaymentMethodNonce = nonce,
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true,
            }
        };

        var result = await _apiGateway.Transaction.SaleAsync(request);
        if (result is null || result?.Errors is not null)
        {
            return Result.Failure("Could not complete payment");
        }

        await _userService.AddMoney(userId, amount);

        return Result.Success();
    }

    public async Task<string> CreateUser(string email, string firstName, string lastName)
    {
        var customerRequest = new CustomerRequest
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
        };

        var createdCustomer = await _apiGateway.Customer.CreateAsync(customerRequest);
        await _userService.SetBrainTreeId(email, createdCustomer?.Target?.Id);

        return createdCustomer?.Target?.Id;
    }
}
