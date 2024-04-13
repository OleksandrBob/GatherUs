using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class GetEventsPossibleForInvitesQuery : IRequest<Result<List<CustomEventDto>, FormattedError>>
{
    internal int OrganizerId { get; init; }

    public class Handler : IRequestHandler<GetEventsPossibleForInvitesQuery, Result<List<CustomEventDto>, FormattedError>>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public Handler(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<Result<List<CustomEventDto>, FormattedError>> Handle(GetEventsPossibleForInvitesQuery request,
            CancellationToken _)
        {
            var events = await _eventService.GetEventsForInvites(request.OrganizerId);

            return _mapper.Map<List<CustomEventDto>>(events);
        }
    }
}
