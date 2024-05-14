using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using GatherUs.Core.Helpers;
using GatherUs.Core.Services;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using Moq;

namespace GatherUs.Tests.UnitTests;

public class UserServiceTests
{
    [Fact]
    public async Task GetByEmailAsync_ValidEmail_ReturnUser()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expected = fixture.Create<User>();
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        unitOfWork
            .Setup(m => m.Users
                .GetByAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    false,
                    It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(expected);

        var userService = new UserService(unitOfWork.Object);

        var user = await userService.GetByEmailAsync(It.IsAny<string>());

        Assert.Same(expected, user);
    }

    [Fact]
    public async Task GetByIdAsync_ValidEmail_ReturnUser()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expected = fixture.Create<User>();
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        unitOfWork
            .Setup(m => m.Users
                .GetByAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    false,
                    It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(expected);

        var userService = new UserService(unitOfWork.Object);

        var user = await userService.GetByIdAsync(It.IsAny<int>());

        Assert.Same(expected, user);
    }

    [Fact]
    public async Task AddMoney_ValidId_AddMoney()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedUser = fixture.Create<User>();
        var expectedBalance = expectedUser.Balance;
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        unitOfWork
            .Setup(m => m.Users
                .GetByAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    false,
                    It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(expectedUser);

        var userService = new UserService(unitOfWork.Object);
        var amountToAdd = It.IsAny<decimal>();

        await userService.AddMoney(expectedUser.Id, amountToAdd);
        Assert.Equal(expectedBalance + amountToAdd, expectedUser.Balance);
    }

    [Fact]
    public async Task SetBrainTreeId_ValidEmail_AddId()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedUser = fixture.Create<User>();
        var expectedBraintreeId = It.IsAny<string>();
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        unitOfWork
            .Setup(m => m.Users
                .GetByAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    false,
                    It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(expectedUser);

        var userService = new UserService(unitOfWork.Object);

        await userService.SetBrainTreeId(expectedUser.Mail, expectedBraintreeId);

        Assert.Equal(expectedBraintreeId, expectedUser.BrainTreeId);
    }

    [Fact]
    public async Task DeleteProfilePicture_UserExists_DeletePicture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedUser = fixture.Create<User>();
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        unitOfWork
            .Setup(m => m.Users
                .GetByAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    false,
                    It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(expectedUser);
        var userService = new UserService(unitOfWork.Object);

        await userService.DeleteProfilePicture(expectedUser.Id);

        Assert.Null(expectedUser.ProfilePictureUrl);
    }

    [Fact]
    public async Task DeleteProfilePicture_UserDontExists_Fail()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        User expectedUser = null;
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        unitOfWork
            .Setup(m => m.Users
                .GetByAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    false,
                    It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(expectedUser);
        var userService = new UserService(unitOfWork.Object);

        var actualResult = await userService.DeleteProfilePicture(It.IsAny<int>());

        Assert.True(actualResult.IsFailure);
    }

    [Fact]
    public async Task UploadProfilePicture_UserExists_UpdatePicture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedUser = fixture.Create<User>();
        var name = "qqqqq.png";
        var imageFile = "sdfg";
        var ext = name?.Split('.')[1];
        var fileName = PictureHelper.GetUserProfilePictureUrl(expectedUser.Id) + '.' + ext;

        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        unitOfWork
            .Setup(m => m.Users
                .GetByAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    false,
                    It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(expectedUser);
        var userService = new UserService(unitOfWork.Object);

        var profilePictureUrl = await userService.UploadProfilePicture(expectedUser.Id, imageFile, name);

        Assert.Equal("https://gatherus.blob.core.windows.net/images/" + fileName, profilePictureUrl);
    }
    
    [Fact]
    public async Task UploadProfilePicture_FileDontExists_UpdatePicture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedUser = fixture.Create<User>();
        var name = It.IsAny<string>();
        var imageFile = It.IsAny<string>();
        var ext = name?.Split('.')[1];
        var fileName = PictureHelper.GetUserProfilePictureUrl(expectedUser.Id) + '.' + ext;

        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();

        unitOfWork
            .Setup(m => m.Users
                .GetByAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    false,
                    It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(expectedUser);
        var userService = new UserService(unitOfWork.Object);

        var profilePictureUrl = await userService.UploadProfilePicture(expectedUser.Id, imageFile, name);

        Assert.True(profilePictureUrl.IsFailure);
    }
}
