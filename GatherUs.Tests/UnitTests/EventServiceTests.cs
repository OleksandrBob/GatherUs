using AutoFixture;
using AutoFixture.AutoMoq;
using GatherUs.Core.Mailing.SetUp;
using GatherUs.Core.Services;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using GatherUs.Enums;
using Moq;

namespace GatherUs.Tests.UnitTests;

public class EventServiceTests
{
    private static AzureOptions AzureOptions => new()
    {
        ConnectionStringConfig =
            "DefaultEndpointsProtocol=https;AccountName=gatherus;AccountKey=L7c5tB9b2UDkYeURe0jL+35lgAPSEwkTq5cwubkmM5kGl+JJeJR062fnOJ7syn3S/sJBLjblSDkq+AStq/Ubcw==;EndpointSuffix=core.windows.net"
    };

    private static WhereByOptions WhereByOptions => new()
    {
        ApiKeyConfig = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmFwcGVhci5pbiIsImF1ZCI6Imh0dHBzOi8vYXBpLmFwcGVhci5pbi92MSIsImV4cCI6OTAwNzE5OTI1NDc0MDk5MSwiaWF0IjoxNzExNDcxNjY1LCJvcmdhbml6YXRpb25JZCI6MjIxMTA4LCJqdGkiOiIzOGQyZTlhZS1jNjk1LTQxMTMtOGVkZi1jNWEzZDIxNDk0NzMifQ._wOeA5JR6WKPlKCUqu158QSlJ17grDVAjbqMlaqwPAc",
        ApiUrlConfig = "https://api.whereby.dev/v1/meetings"
    };
    
    [Fact]
    public async Task CreateEvent_OnsiteEvent_EventCreated()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedEvent = fixture.Create<CustomEvent>();
        var expectedLocation = expectedEvent.Location;
        var expectedLat = expectedEvent.LocationLatitude;
        var expectedLong = expectedEvent.LocationLongitude;
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        var httpClientFactory = fixture.Freeze<Mock<IHttpClientFactory>>();
        expectedEvent.CustomEventLocationType = CustomEventLocationType.Onsite;

        var eventService = new EventService(unitOfWork.Object, httpClientFactory.Object, AzureOptions, WhereByOptions);

        var createdEventId = await eventService.CreateEvent(
            expectedEvent.OrganizerId,
            expectedEvent.Name,
            expectedEvent.Description,
            expectedEvent.StartTimeUtc,
            expectedEvent.MinRequiredAge,
            expectedEvent.TicketPrice,
            expectedEvent.TotalTicketCount,
            null,
            "",
            expectedLocation,
            expectedLat,
            expectedLong,
            expectedEvent.CustomEventType,
            expectedEvent.CustomEventLocationType,
            expectedEvent.CustomEventCategories
        );
        
        Assert.Equal(0, createdEventId);
        Assert.Equal(expectedLocation,expectedEvent.Location);
        Assert.Equal(expectedLat, expectedEvent.LocationLatitude);
        Assert.Equal(expectedLong, expectedEvent.LocationLongitude);
    }

    [Fact]
    public async Task InviteUser_ValidData_InviteCreated()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var expectedEvent = fixture.Create<CustomEvent>();
        var expectedGuestId = It.IsAny<int>();
        var expectedEventId = It.IsAny<int>();
        var expectedInviteMessage = It.IsAny<string>();
        var unitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        var httpClientFactory = fixture.Freeze<Mock<IHttpClientFactory>>();
        expectedEvent.CustomEventLocationType = CustomEventLocationType.Onsite;

        var eventService = new EventService(unitOfWork.Object, httpClientFactory.Object, AzureOptions, WhereByOptions);

        var createdInvite = await eventService.InviteUser(expectedGuestId, expectedEventId, expectedInviteMessage);
        
        Assert.NotNull(createdInvite);
        Assert.Equal(expectedGuestId, createdInvite.GuestId);
        Assert.Equal(expectedEventId, createdInvite.CustomEventId);
    }
}