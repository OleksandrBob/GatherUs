using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using GatherUs.Enums.DAL;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class SetInviteStatusCommand : IRequest<Result>
{
    internal int InviteId { get; set; }

    internal int GuestId { get; set; }

    public InviteStatus InviteStatus { get; set; }

    public class Handler : IRequestHandler<SetInviteStatusCommand, Result>
    {
        private readonly IEventService _eventService;

        public Handler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Result> Handle(SetInviteStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _eventService.SetInviteStatus(request.InviteId, request.InviteStatus);
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }

            return Result.Success();
        }
    }
}
