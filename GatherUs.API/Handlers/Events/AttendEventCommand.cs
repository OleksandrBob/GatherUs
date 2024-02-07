using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class AttendEventCommand : IRequest<Result>
{
    public int EventId { get; init; }

    internal int GuestId { get; init; }

    public class Handler : IRequestHandler<AttendEventCommand, Result>
    {
        private readonly IEventService _eventService;

        public Handler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Result> Handle(AttendEventCommand request, CancellationToken cancellationToken)
        {
            return await _eventService.AddAttendantToEvent(request.EventId, request.GuestId);
        }
    }
}
