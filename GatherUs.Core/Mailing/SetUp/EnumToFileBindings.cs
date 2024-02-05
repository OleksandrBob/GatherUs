namespace GatherUs.Core.Mailing.SetUp;

public static class EnumToFileBindings
{
    public static string GetFileName(MailType mail)
    {
        return mail switch
        {
            MailType.OrganizerVerification => "OrganizerVerification",
            MailType.GuestVerification => "GuestVerification",
            MailType.ConfirmationCode => "ConfirmationCode",
            MailType.AttendanceInvite => "AttendanceInvite",
            _ => string.Empty,
        };
    }
}