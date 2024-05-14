using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using GatherUs.Core.Services;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using GatherUs.Enums;
using Moq;

namespace GatherUs.Tests.UnitTests;

public class EmailForRegistrationServiceTests
{
    [Fact]
    public async Task AddEmailForRegistration_ValidEmail_EmailAdded()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedEmail = fixture.Create<EmailForRegistration>();
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        unitOfWork
            .Setup(m => m.EmailForRegistrations
                .GetByAsync(
                    It.IsAny<Expression<Func<EmailForRegistration, bool>>>(),
                    false,
                    It.IsAny<Expression<Func<EmailForRegistration, object>>[]>()))
            .ReturnsAsync(expectedEmail);

        var emailForRegistrationService = new EmailForRegistrationService(unitOfWork.Object);
        var email = It.IsAny<string>();
        var userType = It.IsAny<UserType>();
        var actualResult =
            await emailForRegistrationService.AddEmailForRegistration(email, userType);

        Assert.Equal(actualResult.Value.Email, email);
        Assert.Equal(actualResult.Value.UserType, userType);
        Assert.True(actualResult.Value.ConfirmationCode > 1111);
    }
}
