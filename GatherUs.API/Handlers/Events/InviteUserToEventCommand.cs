using CSharpFunctionalExtensions;
using GatherUs.Core.Mailing.SetUp;
using GatherUs.Core.RabbitMq;
using GatherUs.Core.RabbitMq.Interfaces;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.Enums.DAL;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class InviteUserToEventCommand : IRequest<Result>
{
    internal int OrganizerId { get; set; }

    public int CustomEventId { get; set; }

    public string GuestEmail { get; set; }

    public string Message { get; set; }

    public class Handler : IRequestHandler<InviteUserToEventCommand, Result>
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly IMessagePublisher _messagePublisher;

        public Handler(
            IUserService userService,
            IEventService eventService,
            IMessagePublisher messagePublisher)
        {
            _userService = userService;
            _eventService = eventService;
            _messagePublisher = messagePublisher;
        }

        public async Task<Result> Handle(InviteUserToEventCommand request, CancellationToken cancellationToken)
        {
            var organizer = await _userService.GetByIdAsync(request.OrganizerId);

            if (organizer is null)
            {
                return Result.Failure("Organizer with specified id doesn't exist.");
            }

            if (organizer.UserType != UserType.Organizer)
            {
                return Result.Failure("You can`t invite to the event if you are not an organizer.");
            }

            var guest = await _userService.GetByEmailAsync(request.GuestEmail);

            if (guest is null)
            {
                return Result.Failure("Guest with specified email doesn't exist.");
            }

            if (guest.UserType != UserType.Guest)
            {
                return Result.Failure("You can`t invite non Guest to the event.");
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

            if (customEvent.CustomEventType == CustomEventType.Event)
            {
                return Result.Failure("You can only invite to a 'Conference' or a 'Meeting'.");
            }
            
            var guests = await _eventService.GetGuestsInvitedToEvent(request.CustomEventId);

            if (guests.Any(g => g.Mail == request.GuestEmail))
            {
                return Result.Failure("Guest is already invited.");
            }

            try
            {
                var invite = await _eventService.InviteUser(guest.Id, request.CustomEventId, request.Message);

                invite.Guest = guest as Guest;
                invite.CustomEvent = customEvent;

                _messagePublisher.PublishMessage(new QueueMessage
                {
                    Type = MailType.AttendanceInvite,
                    MessageValue = invite,
                });
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }

            return Result.Success();
        }
    }
}
