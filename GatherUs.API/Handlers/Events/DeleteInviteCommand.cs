using CSharpFunctionalExtensions;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class DeleteInviteCommand : IRequest<Result<int, FormattedError>>
{
    internal int OrganizerId { get; init; }

    internal int InviteId { get; init; }

    public class Handler : IRequestHandler<DeleteInviteCommand, Result<int, FormattedError>>
    {
        private readonly IEventService _eventService;

        public Handler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Result<int, FormattedError>> Handle(DeleteInviteCommand request, CancellationToken ct)
        {
            var invite = await _eventService.GetInvite(request.InviteId, request.OrganizerId);

            if (invite is null)
            {
                return Result.Failure<int, FormattedError>(new("invite does not exist"));
            }

            await _eventService.DeleteInvite(invite.Id);
            return Result.Success<int, FormattedError>(invite.Id);
        }
    }
}
