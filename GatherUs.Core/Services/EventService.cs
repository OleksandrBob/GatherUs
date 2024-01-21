using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using GatherUs.Enums.DAL;
using Microsoft.CodeAnalysis.CSharp;

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
}
