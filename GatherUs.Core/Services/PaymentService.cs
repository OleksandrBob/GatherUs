using GatherUs.Core.Services.Interfaces;

namespace GatherUs.Core.Services;
using Braintree;

public class PaymentService : IPaymentService
{
    private readonly BraintreeGateway _apiGateway;

    public PaymentService()
    {
        //TODO: get params fom env
        _apiGateway = new()
        {
            Environment = Environment.SANDBOX,
            MerchantId = "vsk2ttpb6t6wcdb4",
            PublicKey = "ybtn3jmc3ynv3cbm",
            PrivateKey = "8256f637a827408c535bfaa75f2908ce"
        };
    }

    public string GenerateClientToken(int userId)
    {
        return _apiGateway.ClientToken.Generate(new ClientTokenRequest
        {
            CustomerId = "19865428271",
        });
    }

    public async Task ProcessPayment(string nonce)
    {
        TransactionRequest request = new TransactionRequest
        {
            Amount = 1000.00M,
            PaymentMethodNonce = nonce,
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true,
            }
        };

        Result<Transaction> result = await _apiGateway.Transaction.SaleAsync(request);
    }

    public string CreateCustomer()
    {
        
        /*
         *         var customerRequest = new CustomerRequest
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
        };
        var r =_apiGateway.Customer.CreateAsync(customerRequest);
         */
        return "";
    }
}
