using CSharpFunctionalExtensions;
using GatherUs.DAL.Models;
using GatherUs.Enums.DAL;

namespace GatherUs.Core.Services.Interfaces;

public interface IEventService
{
    Task<int> CreateEvent(
        int organizerId,
        string name,
        string description,
        DateTime startTimeUtc,
        byte minRequiredAge,
        decimal ticketPrice,
        int ticketCount,
        string image,
        string imageName,
        CustomEventType customEventType,
        CustomEventLocationType customEventLocationType,
        List<CustomEventCategory> customEventCategories);

    Task<List<CustomEvent>> GetEventsByUserId(int userId, bool showPastEvents = false);

    Task<CustomEvent> GetEventById(int eventId);

    Task<AttendanceInvite> InviteUser(int guestId, int customEventId, string inviteMessage);

    Task<List<AttendanceInvite>> GetGuestInvites(int guestId, InviteStatus inviteStatus);

    Task<List<AttendanceInvite>> GetSentInvites(int organizerId, InviteStatus inviteStatus = InviteStatus.Pending);

    Task<List<Guest>> GetGuestsInvitedToEvent(int customEventId);

    Task<List<CustomEvent>> GetFilteredEvents(
        string searchString,
        DateTime? fromDate,
        DateTime? toDate,
        byte? fromMinRequiredAge,
        byte? toMinRequiredAge,
        decimal? fromTicketPrice,
        decimal? toTicketPrice,
        List<CustomEventType> customEventTypes,
        List<CustomEventLocationType> customEventLocationTypes,
        List<CustomEventCategory> customEventCategories);

    Task<Result<AttendanceInvite>> SetInviteStatus(AttendanceInvite invite, InviteStatus newStatus);

    Task<Result> AddAttendantToEvent(int customEventId, int guestId);

    Task<AttendanceInvite> GetInvite(int inviteId, int? organizerId = null);

    Task DeleteInvite(int inviteId);
}
