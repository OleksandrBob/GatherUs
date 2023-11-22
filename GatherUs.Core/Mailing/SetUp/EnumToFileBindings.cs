using System.Diagnostics;

namespace GatherUs.Core.Mailing.SetUp;

public static class EnumToFileBindings
{
    public static string GetFileName(MailType mail)
    {
        return mail switch
        {
            MailType.OrganizerVerification => "OrganizerVerification",
            MailType.GuestVerification => "GuestVerification",
            _ => string.Empty,
        };
    }
}