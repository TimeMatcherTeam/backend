using Microsoft.AspNetCore.Identity;

namespace TimeMatcher.Domain.UserAggregate;

public class UserRole : IdentityRole<Guid>
{
    internal UserRole()
    {
    }

    public UserRole(string roleName) : base(roleName)
    {
    }
}