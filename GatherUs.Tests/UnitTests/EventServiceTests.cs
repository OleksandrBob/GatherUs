using AutoFixture;
using AutoFixture.AutoMoq;
using GatherUs.Core.Services;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using GatherUs.Enums;
using Moq;

namespace GatherUs.Tests.UnitTests;

public class EventServiceTests
{
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

        var eventService = new EventService(unitOfWork.Object, httpClientFactory.Object);

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

        var eventService = new EventService(unitOfWork.Object, httpClientFactory.Object);

        var createdInvite = await eventService.InviteUser(expectedGuestId, expectedEventId, expectedInviteMessage);
        
        Assert.NotNull(createdInvite);
        Assert.Equal(expectedGuestId, createdInvite.GuestId);
        Assert.Equal(expectedEventId, createdInvite.CustomEventId);
    }
}