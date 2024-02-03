using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using GatherUs.Enums.DAL;
using MediatR;

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

    public List<CustomEventType> CustomEventTypes { get; set; }

    public List<CustomEventLocationType> CustomEventLocationTypes { get; set; }

    public List<CustomEventCategory> CustomEventCategories { get; set; }

    public class Handler : IRequestHandler<SearchEventQuery, Result<List<CustomEventDto>, FormattedError>>
    {
        private readonly IEventService _eventService;

        public Handler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public Task<Result<List<CustomEventDto>, FormattedError>> Handle(SearchEventQuery request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
