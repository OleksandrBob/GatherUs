using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class GetEventInfoQuery : IRequest<Result<CustomEventDto, FormattedError>>
{
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

            return _mapper.Map<CustomEventDto>(eventInfo);
        }
    }
}
