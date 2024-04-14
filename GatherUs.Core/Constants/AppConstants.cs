namespace GatherUs.Core.Constants;

public static class AppConstants
{
    public static string Salt => "VeniVidiViciVeniVidiViciVeniVidiViciVeniVidiViciVeniVidiVici";

    public static string JwtAudience => "GatherUsClient";

    public static string JwtIssuer => "GatherUsServer";

    public const string OrganizerRole = "Organizer";

    public const string GuestRole =  "Guest";
    
    public const string BearerAuth  = "bearerAuth";
}
