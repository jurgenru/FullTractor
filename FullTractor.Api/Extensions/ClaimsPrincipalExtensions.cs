using System.Security.Claims;

namespace FullTractor.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    // The token's "sub" claim is mapped to NameIdentifier by the JWT bearer handler.
    public static int GetUserId(this ClaimsPrincipal user) =>
        int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole("Admin");

    /// <summary>True when the caller is that user or an admin.</summary>
    public static bool CanAccessUser(this ClaimsPrincipal user, int userId) =>
        user.IsAdmin() || user.GetUserId() == userId;
}
