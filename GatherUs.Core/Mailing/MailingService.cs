using CSharpFunctionalExtensions;
using GatherUs.Core.Mailing.Dto;
using GatherUs.Core.Mailing.SetUp;
using GatherUs.DAL.Models;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace GatherUs.Core.Mailing;

public class MailingService : IMailingService
{
    private readonly ISmtpOptions _smtpOptions;

    public MailingService(ISmtpOptions smtpOptions)
    {
        _smtpOptions = smtpOptions;
    }

    public async Task<Result> SendGuestVerificationMailAsync(Guest guest)
    {
        return await SendMailAsync(
            to: guest.Mail,
            subject: "Welcome!",
            from: "GatherUs",
            content: await MailingHelper.GenerateTemplate(MailType.GuestVerification, new UserForMailDto(guest)));
    }

    public async Task<Result> SendOrganizerVerificationMailAsync(Organizer organizer)
    {
        return await SendMailAsync(
            to: organizer.Mail,
            subject: "Welcome!",
            from: "GatherUs",
            content: await MailingHelper.GenerateTemplate(MailType.OrganizerVerification,
                new UserForMailDto(organizer)));
    }

    public async Task<Result> SendMailVerificationCodeAsync(EmailForRegistration emailForRegistration)
    {
        return await SendMailAsync(
            to: emailForRegistration.Email,
            subject: "Confirm email",
            from: "GatherUs",
            content: await MailingHelper.GenerateTemplate(MailType.ConfirmationCode,
                emailForRegistration.ConfirmationCode));
    }

    public async Task<Result> SendInviteMailAsync(AttendanceInvite invite)
    {
        return await SendMailAsync(
            to: invite.Guest.Mail,
            subject: "Invitation!",
            from: invite.CustomEvent.Name,
            content: await MailingHelper.GenerateTemplate(MailType.AttendanceInvite,
                new AttendanceDto(invite)));
    }

    private async Task<Result> SendMailAsync(string to, string subject, string content, string from)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(from, _smtpOptions.UserName));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = content,
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpOptions.UserName, _smtpOptions.Password);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}
