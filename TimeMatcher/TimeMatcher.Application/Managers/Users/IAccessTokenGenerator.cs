using System.Security.Claims;

namespace TimeMatcher.Application.Managers.Users;

public interface IAccessTokenGenerator
{
    string Generate(IEnumerable<Claim> claims);
}