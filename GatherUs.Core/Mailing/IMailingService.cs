using CSharpFunctionalExtensions;
using GatherUs.Core.Mailing.Dto;
using GatherUs.DAL.Models;

namespace GatherUs.Core.Mailing;

public interface IMailingService
{
    Task<Result> SendInviteMailAsync(AttendanceInvite invite);

    Task<Result> SendGuestVerificationMailAsync(Guest guest);

    Task<Result> SendOrganizerVerificationMailAsync(Organizer organizer);


    Task<Result> SendMailVerificationCodeAsync(EmailForRegistration emailForRegistration);
}
