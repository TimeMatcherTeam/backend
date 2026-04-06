using Microsoft.AspNetCore.Identity;

namespace TimeMatcher.Domain.UserAggregate;

public class User: IdentityUser<Guid>
{
    public string Nickname { get; set; }
    public Calendar Calendar { get; init; } = new();
}