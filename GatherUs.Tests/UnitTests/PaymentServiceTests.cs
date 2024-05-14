using AutoFixture;
using AutoFixture.AutoMoq;
using Braintree;
using GatherUs.Core.Services;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using Moq;

namespace GatherUs.Tests.UnitTests;

public class PaymentServiceTests
{
    [Fact]
    public async Task GenerateClientToken_ValidId_ReturnToken()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedToken = It.IsAny<string>();
        var expectedUser = fixture.Create<User>();
        var userService = fixture.Freeze<Mock<IUserService>>();
        var braintreeGateway = fixture.Freeze<Mock<BraintreeGateway>>();

        userService
            .Setup(m => m.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(expectedUser);

        braintreeGateway
            .Setup(m => m.ClientToken.GenerateAsync(new ClientTokenRequest
            {
                CustomerId = expectedUser.BrainTreeId,
            }))
            .ReturnsAsync(expectedToken);

        var paymentService = new PaymentService(userService.Object, braintreeGateway.Object);

        var token = await paymentService.GenerateClientToken(expectedUser.Id);

        Assert.Equal(expectedToken, token);
    }

    [Fact]
    public async Task CreateUser_Success_ReturnCreatedId()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedCustomer = It.IsAny<Result<Customer>>();
        var usr = fixture.Create<User>();
        var userService = fixture.Freeze<Mock<IUserService>>();
        var braintreeGateway = fixture.Freeze<Mock<BraintreeGateway>>();

        userService
            .Setup(m => m.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(usr);

        braintreeGateway
            .Setup(m => m.Customer.CreateAsync(It.IsAny<CustomerRequest>()))
            .ReturnsAsync(expectedCustomer);

        var paymentService = new PaymentService(userService.Object, braintreeGateway.Object);
        var newUserId = await paymentService.CreateUser(usr.Mail, usr.FirstName, usr.LastName);

        Assert.Equal(expectedCustomer?.Target?.Id, newUserId);
    }

    [Fact]
    public async Task ReplenishBalance_Success_ReturnCreatedId()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var braintreeGateway = fixture.Freeze<Mock<BraintreeGateway>>();
        var expectedNullTransaction = It.IsAny<Result<Transaction>>();
        var usr = fixture.Create<User>();
        var userService = fixture.Freeze<Mock<IUserService>>();
        
        braintreeGateway
            .Setup(m => m.Transaction.SaleAsync(It.IsAny<TransactionRequest>()))
            .ReturnsAsync(expectedNullTransaction);
        
        var paymentService = new PaymentService(userService.Object, braintreeGateway.Object);
        var actualResult = await paymentService.ReplenishBalance(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>());
        
        Assert.True(actualResult.IsFailure);
    }
}