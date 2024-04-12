using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.API.DTO.User;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using GatherUs.Enums.DAL;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class GetGuestInvitesQuery : IRequest<Result<List<AttendanceInviteDto>, FormattedError>>
{
    internal int GuestId { get; set; }

    public InviteStatus InviteStatus { get; set; } = InviteStatus.Pending;

    public class Handler : IRequestHandler<GetGuestInvitesQuery, Result<List<AttendanceInviteDto>, FormattedError>>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public Handler(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }
        
        public async Task<Result<List<AttendanceInviteDto>, FormattedError>> Handle(GetGuestInvitesQuery request, CancellationToken cancellationToken)
        {
            var invites = await _eventService.GetGuestInvites(request.GuestId, request.InviteStatus);
            var invitesDto = new List<AttendanceInviteDto>(invites.Count);

            foreach (var inv in invites)
            {
                var invDto =  _mapper.Map<AttendanceInviteDto>(inv);
                invDto.SmallEventDto = _mapper.Map<SmallEventDto>(inv.CustomEvent);
                invDto.UserDto = _mapper.Map<UserDto>(inv.CustomEvent.Organizer);
                invitesDto.Add(invDto);
            }

            return invitesDto;
        }
    }
}
