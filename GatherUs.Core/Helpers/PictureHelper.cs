namespace GatherUs.Core.Helpers;

public static class PictureHelper
{
    public static string GetUserProfilePictureUrl(int userId) => $"user_profile_pic_{userId}";
}
