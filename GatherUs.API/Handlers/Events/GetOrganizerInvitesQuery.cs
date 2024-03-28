using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.API.DTO.User;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class GetOrganizerInvitesQuery : IRequest<Result<List<AttendanceInviteDto>, FormattedError>>
{
    internal int OrganizerId { get; set; }

    public class Handler : IRequestHandler<GetOrganizerInvitesQuery, Result<List<AttendanceInviteDto>, FormattedError>>
    {
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public Handler(IMapper mapper, IEventService eventService)
        {
            _mapper = mapper;
            _eventService = eventService;
        }

        public async Task<Result<List<AttendanceInviteDto>, FormattedError>> Handle(GetOrganizerInvitesQuery request,
            CancellationToken ct)
        {
            var invites = await _eventService.GetSentInvites(request.OrganizerId);
            var invitesDto = new List<AttendanceInviteDto>(invites.Count);

            foreach (var inv in invites)
            {
                var invDto =  _mapper.Map<AttendanceInviteDto>(inv);
                invDto.SmallEventDto = _mapper.Map<SmallEventDto>(inv.CustomEvent);
                invDto.UserDto = _mapper.Map<UserDto>(inv.Guest);
                invitesDto.Add(invDto);
            }

            return invitesDto;
        }
    }
}
