using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Repository;
using GatherUs.Enums.DAL;
using MediatR;

namespace GatherUs.API.Handlers.Events;

public class SetInviteStatusCommand : IRequest<Result>
{
    internal int InviteId { get; set; }

    public InviteStatus InviteStatus { get; set; }

    public class Handler : IRequestHandler<SetInviteStatusCommand, Result>
    {
        private readonly IEventService _eventService;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IEventService eventService, IUnitOfWork unitOfWork)
        {
            _eventService = eventService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetInviteStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.InviteStatus == InviteStatus.Pending)
            {
                return Result.Failure("Cannot choose this status");
            }

            try
            {
                var invite = await _unitOfWork.AttendanceInvites.GetFirstOrDefaultAsync(i => i.Id == request.InviteId);

                if (request.InviteStatus == InviteStatus.Accepted)
                {
                    var addingResult = await _eventService.AddAttendantToEvent(invite.CustomEventId, invite.GuestId);

                    if (addingResult.IsFailure)
                    {
                        return addingResult;
                    }
                }

                var inv = await _eventService.SetInviteStatus(invite, request.InviteStatus);

                if (inv.IsFailure)
                {
                    return inv;
                }
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }

            return Result.Success();
        }
    }
}
