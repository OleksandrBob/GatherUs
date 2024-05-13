using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using GatherUs.Enums;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class GetEventInfoQuery : IRequest<Result<CustomEventForUserDto, FormattedError>>
{
    internal int UserId { get; init; }

    public int EventId { get; init; }

    public class Handler : IRequestHandler<GetEventInfoQuery, Result<CustomEventForUserDto, FormattedError>>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public Handler(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<Result<CustomEventForUserDto, FormattedError>> Handle(GetEventInfoQuery request,
            CancellationToken ct)
        {
            var eventInfo = await _eventService.GetEventById(request.EventId, e => e.CustomEventGuests);

            var eventDto = _mapper.Map<CustomEventForUserDto>(eventInfo);
            eventDto.RoomUrl = null;

            if (eventInfo.OrganizerId == request.UserId)
            {
                eventDto.RoomUrl = eventInfo.HostRoomUrl;
                eventDto.UserType = UserType.Organizer;
            }
            else
            {
                eventDto.UserType = UserType.Guest;

                if (eventInfo.CustomEventGuests.Select(g => g.GuestId).Contains(request.UserId))
                {
                    eventDto.RoomUrl = eventInfo.RoomUrl;
                    eventDto.IsUserAttending = true;
                }
            }

            return eventDto;
        }
    }
}
