using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.Event;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class CreateEventCommand : IRequest<Result<int, FormattedError>>
{
    internal int OrganizerId { get; set; }

    [Required] public CustomEventDto EventDto { get; set; }

    public class Handler : IRequestHandler<CreateEventCommand, Result<int, FormattedError>>
    {
        private readonly IEventService _eventService;

        public Handler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Result<int, FormattedError>> Handle(CreateEventCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdEventId = await _eventService.CreateEvent(
                    request.OrganizerId,
                    request.EventDto.Name,
                    request.EventDto.Description,
                    request.EventDto.StartTimeUtc,
                    request.EventDto.MinRequiredAge,
                    request.EventDto.TicketPrice,
                    request.EventDto.CustomEventType,
                    request.EventDto.CustomEventLocationType,
                    request.EventDto.CustomEventCategories);

                return Result.Success<int, FormattedError>(createdEventId);
            }
            catch (Exception ex)
            {
                return Result.Failure<int, FormattedError>(new(ex.Message));
            }
        }
    }
}
