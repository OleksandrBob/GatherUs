namespace GatherUs.Core.Helpers;

public static class PictureHelper
{
    public static string GetUserProfilePictureUrl(int userId) => $"user_profile_pic_{userId}";
    
    public static string GetEventPictureUrl(int eventId) => $"event_pic_{eventId}";
}
