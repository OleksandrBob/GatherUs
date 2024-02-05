using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using GatherUs.Enums.DAL;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace GatherUs.Core.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;

    public EventService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CreateEvent(
        int organizerId,
        string name,
        string description,
        DateTime startTimeUtc,
        byte minRequiredAge,
        decimal ticketPrice,
        CustomEventType customEventType,
        CustomEventLocationType customEventLocationType,
        List<CustomEventCategory> customEventCategories)
    {
        var eventToCreate = new CustomEvent
        {
            Name = name,
            OrganizerId = organizerId,
            Description = description,
            TicketPrice = ticketPrice,
            StartTimeUtc = startTimeUtc,
            MinRequiredAge = minRequiredAge,
            CustomEventType = customEventType,
            CustomEventCategories = customEventCategories,
            CustomEventLocationType = customEventLocationType,
        };

        _unitOfWork.CustomEvents.AddNew(eventToCreate);
        await _unitOfWork.CompleteAsync();

        return eventToCreate.Id;
    }

    public async Task<List<CustomEvent>> GetEventsByUserId(int userId, bool showPastEvents = false)
    {
        if (showPastEvents)
        {
            return await _unitOfWork.CustomEvents.GetAllAsync(e => e.StartTimeUtc < DateTime.UtcNow &&
                                                                   (e.OrganizerId == userId ||
                                                                    e.Attendants.Any(a => a.Id == userId)));
        }

        return await _unitOfWork.CustomEvents.GetAllAsync(e => e.StartTimeUtc >= DateTime.UtcNow &&
                                                               (e.OrganizerId == userId ||
                                                                e.Attendants.Any(a => a.Id == userId)));
    }

    public async Task<CustomEvent> GetEventById(int eventId)
    {
        return await _unitOfWork.CustomEvents.GetFirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task<AttendanceInvite> InviteUser(int guestId, int customEventId, string inviteMessage)
    {
        AttendanceInvite invite = new()
        {
            GuestId = guestId,
            Message = inviteMessage,
            CustomEventId = customEventId,
            InviteStatus = InviteStatus.Pending,
        };

        _unitOfWork.AttendanceInvites.AddNew(invite);
        await _unitOfWork.CompleteAsync();

        return invite;
    }

    public async Task<List<AttendanceInvite>> GetGuestInvites(int guestId, InviteStatus inviteStatus)
    {
        return await _unitOfWork.AttendanceInvites.GetAllAsync(e =>
            e.GuestId == guestId && e.InviteStatus == inviteStatus);
    }

    public async Task<List<Guest>> GetGuestsInvitedToEvent(int customEventId)
    {
        return await _unitOfWork.AttendanceInvites.GetSelectAsync(
            selector: e => e.Guest,
            predicate: e => e.CustomEventId == customEventId,
            include: i => i.Include(ee => ee.Guest));
    }

    public async Task<List<CustomEvent>> GetFilteredEvents(
        string searchString,
        DateTime? fromDate,
        DateTime? toDate,
        byte? fromMinRequiredAge,
        byte? toMinRequiredAge,
        decimal? fromTicketPrice,
        decimal? toTicketPrice,
        List<CustomEventType> customEventTypes,
        List<CustomEventLocationType> customEventLocationTypes,
        List<CustomEventCategory> customEventCategories)
    {
        var predicate = PredicateBuilder.New<CustomEvent>(c => c.CustomEventType != CustomEventType.Meeting);

        if (searchString is not null)
        {
            predicate = predicate.And(e =>
                e.Name.ToLower().Contains(searchString) || e.Description.ToLower().Contains(searchString));
        }

        if (fromDate.HasValue)
        {
            predicate = predicate.And(e => e.StartTimeUtc >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            predicate = predicate.And(e => e.StartTimeUtc <= toDate.Value);
        }

        if (fromMinRequiredAge.HasValue)
        {
            predicate = predicate.And(e => e.MinRequiredAge >= fromMinRequiredAge);
        }

        if (toMinRequiredAge.HasValue)
        {
            predicate = predicate.And(e => e.MinRequiredAge <= toMinRequiredAge);
        }

        if (fromTicketPrice.HasValue)
        {
            predicate = predicate.And(e => e.TicketPrice >= fromTicketPrice);
        }

        if (toTicketPrice.HasValue)
        {
            predicate = predicate.And(e => e.TicketPrice <= toTicketPrice);
        }

        if (customEventTypes is not null)
        {
            predicate = predicate.And(e => customEventTypes.Contains(e.CustomEventType));
        }

        if (customEventLocationTypes is not null)
        {
            predicate = predicate.And(e => customEventLocationTypes.Contains(e.CustomEventLocationType));
        }

        if (customEventCategories is not null)
        {
            var pred = PredicateBuilder.New<CustomEvent>(false);

            foreach (var category in customEventCategories)
            {
                pred = pred.Or(c => c.CustomEventCategories.Contains(category));
            }

            predicate = predicate.And(pred);
        }

        return await _unitOfWork.CustomEvents.GetAllAsync(predicate);
    }

    public async Task<InviteStatus> SetInviteStatus(int inviteId, InviteStatus newStatus)
    {
        var invite = await _unitOfWork.AttendanceInvites.GetFirstOrDefaultAsync(i => i.Id == inviteId);
        invite.InviteStatus = newStatus;
        _unitOfWork.AttendanceInvites.Update(invite);

        await _unitOfWork.CompleteAsync();

        return invite.InviteStatus;
    }
}
