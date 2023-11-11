using System.Security.Claims;
using GatherUs.Core.Constants;

namespace GatherUs.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetLoggedInUserId(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        string userIdString = principal.FindFirstValue(GatherUsClaims.Id);

        if (!int.TryParse(userIdString, out int userId))
        {
            throw new("Cannot convert to int user Id.");
        }

        return userId;
    }
}
