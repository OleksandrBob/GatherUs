using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using GatherUs.Enums.DAL;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace GatherUs.API.Handlers.Events;

public class SearchEventQuery : IRequest<Result<List<CustomEventDto>, FormattedError>>
{
    public string? SearchString { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public byte? FromMinRequiredAge { get; set; }

    public byte? ToMinRequiredAge { get; set; }

    public decimal? FromTicketPrice { get; set; }

    public decimal? ToTicketPrice { get; set; }

    [SwaggerParameter(Required = false)]
    public List<CustomEventType>? CustomEventTypes { get; set; } = null;

    [SwaggerParameter(Required = false)]
    public List<CustomEventLocationType>? CustomEventLocationTypes { get; set; } = null;

    [SwaggerParameter(Required = false)]
    public List<CustomEventCategory>? CustomEventCategories { get; set; } = null;

    public class Handler : IRequestHandler<SearchEventQuery, Result<List<CustomEventDto>, FormattedError>>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public Handler(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<Result<List<CustomEventDto>, FormattedError>> Handle(SearchEventQuery request,
            CancellationToken cancellationToken)
        {
            if (request.FromDate.HasValue)
            {
                request.FromDate = DateTime.SpecifyKind(request.FromDate.Value, DateTimeKind.Utc);
            }
            
            if (request.ToDate.HasValue)
            {
                request.ToDate = DateTime.SpecifyKind(request.ToDate.Value, DateTimeKind.Utc);
            }

            var events = await _eventService.GetFilteredEvents(
                request.SearchString,
                request.FromDate,
                request.ToDate,
                request.FromMinRequiredAge,
                request.ToMinRequiredAge,
                request.FromTicketPrice,
                request.ToTicketPrice,
                request.CustomEventTypes,
                request.CustomEventLocationTypes,
                request.CustomEventCategories);

            return _mapper.Map<List<CustomEventDto>>(events);
        }
    }
}
