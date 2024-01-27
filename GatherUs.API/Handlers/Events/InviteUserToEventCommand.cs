using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using GatherUs.Enums.DAL;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class InviteUserToEventCommand : IRequest<Result>
{
    internal int OrganizerId { get; set; }

    public int CustomEventId { get; set; }

    public int GuestId { get; set; }

    public string Message { get; set; }

    public class Handler : IRequestHandler<InviteUserToEventCommand, Result>
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public Handler(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }

        public async Task<Result> Handle(InviteUserToEventCommand request, CancellationToken cancellationToken)
        {
            var organizer = await _userService.GetByIdAsync(request.OrganizerId);

            if (organizer is null)
            {
                return Result.Failure("User with specified id doesn't exist.");
            }

            if (organizer.UserType != UserType.Organizer)
            {
                return Result.Failure("You can`t invite to the event if you are not an organizer.");
            }

            var customEvent = await _eventService.GetEventById(request.CustomEventId);

            if (customEvent is null)
            {
                return Result.Failure("You can`t invite to the an inexisting event.");
            }

            if (customEvent.OrganizerId != organizer.Id)
            {
                return Result.Failure("You can`t invite to the an inexisting event.");
            }

            try
            {
                await _eventService.InviteUser(request.GuestId, request.CustomEventId, request.Message);
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }

            return Result.Success();
        }
    }
}
