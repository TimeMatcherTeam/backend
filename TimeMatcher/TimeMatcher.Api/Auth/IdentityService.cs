using System.Security.Claims;

namespace TimeMatcher.Api.Auth;

internal class IdentityService(IHttpContextAccessor context) : IIdentityService
{
    public Guid GetUserIdentity()
    {
        return Guid.TryParse(context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : Guid.Empty;
    }
}