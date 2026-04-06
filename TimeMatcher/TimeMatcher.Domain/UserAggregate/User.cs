using Microsoft.AspNetCore.Identity;

namespace TimeMatcher.Domain.UserAggregate;

public class User: IdentityUser<Guid>
{
    public Calendar Calendar { get; init; } = new();
}