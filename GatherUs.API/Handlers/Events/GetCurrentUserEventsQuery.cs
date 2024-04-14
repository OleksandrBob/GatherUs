using System.ComponentModel;
using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class GetCurrentUserEventsQuery : IRequest<Result<List<CustomEventDto>, FormattedError>>
{
    internal int UserId { get; set; }

    [DefaultValue(false)]
    public bool ShowPastEvents { get; set; } = false;

    public class Handler : IRequestHandler<GetCurrentUserEventsQuery, Result<List<CustomEventDto>, FormattedError>>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public Handler(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<Result<List<CustomEventDto>, FormattedError>> Handle(
            GetCurrentUserEventsQuery request,
            CancellationToken cancellationToken)
        {
            var events = await _eventService.GetEventsByUserId(request.UserId, request.ShowPastEvents);

            return _mapper.Map<List<CustomEventDto>>(events);
        }
    }
}
