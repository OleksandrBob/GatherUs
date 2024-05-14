using AutoFixture;
using AutoFixture.AutoMoq;
using GatherUs.Core.Helpers;
using GatherUs.Core.Services;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using Moq;

namespace GatherUs.Tests.UnitTests;

public class OrganizerServiceTests
{
    [Fact]
    public async Task InsertAsync_UserInserted()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var createdUser = fixture.Create<Organizer>();
        var expectedPassword = CryptoHelper.GenerateSaltedHash(createdUser.Password);
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        var organizerService = new OrganizerService(unitOfWork.Object);
        await organizerService.InsertAsync(createdUser);
        
        Assert.Equal(expectedPassword, createdUser.Password);
    }
}
