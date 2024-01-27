using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Guest;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class GetInvitedGuestsToEventQuery : IRequest<Result<List<GuestDto>, FormattedError>>
{
    internal int OrganizerId { get; init; }

    public int CustomEventId { get; init; }

    public class Handler : IRequestHandler<GetInvitedGuestsToEventQuery, Result<List<GuestDto>, FormattedError>>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public Handler(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<Result<List<GuestDto>, FormattedError>> Handle(
            GetInvitedGuestsToEventQuery request,
            CancellationToken cancellationToken)
        {
            var customEvent = await _eventService.GetEventById(request.CustomEventId);

            if (customEvent is null)
            {
                return Result.Failure<List<GuestDto>, FormattedError>(new("Event with specified id doesn't exist."));
            }

            if (customEvent.OrganizerId != request.OrganizerId)
            {
                return Result.Failure<List<GuestDto>, FormattedError>(
                    new("You can`t get access to another organizer's event."));
            }

            var invitedGuests = await _eventService.GetGuestsInvitedToEvent(request.CustomEventId);

            return _mapper.Map<List<GuestDto>>(invitedGuests);
        }
    }
}
