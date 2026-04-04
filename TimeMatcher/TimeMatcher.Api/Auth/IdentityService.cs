using System.Security.Claims;

namespace TimeMatcher.Api.Auth;

public class IdentityService(IHttpContextAccessor context) : IIdentityService
{
    public Guid GetUserIdentity()
    {
        return Guid.TryParse(context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : Guid.Empty;
    }
}