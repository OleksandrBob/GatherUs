using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using GatherUs.Enums.DAL;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class GetEventInfoQuery : IRequest<Result<CustomEventDto, FormattedError>>
{
    internal int UserId { get; set; }
    
    public int EventId { get; set; }

    public class Handler : IRequestHandler<GetEventInfoQuery, Result<CustomEventDto, FormattedError>>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public Handler(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<Result<CustomEventDto, FormattedError>> Handle(GetEventInfoQuery request,
            CancellationToken ct)
        {
            var eventInfo = await _eventService.GetEventById(request.EventId);

            var eventDto = _mapper.Map<CustomEventDto>(eventInfo);
            eventDto.RoomUrl = null;

            if (eventInfo.OrganizerId == request.UserId)
            {
                eventDto.RoomUrl = eventInfo.HostRoomUrl;
            }
            else
            {
                var invitedGuests = await _eventService.GetGuestsInvitedToEvent(eventInfo.Id);
                
                if (invitedGuests.Select(g => g.Id).Contains(request.UserId))
                {
                    eventDto.RoomUrl = eventInfo.RoomUrl;
                }
            }

            return eventDto;
        }
    }
}
