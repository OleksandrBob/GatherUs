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
        CustomEventType customEventType,
        CustomEventLocationType customEventLocationType,
        List<CustomEventCategory> customEventCategories);

    Task<List<CustomEvent>> GetEventsByUserId(int userId, bool showPastEvents = false);
}