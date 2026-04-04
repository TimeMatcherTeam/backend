namespace TimeMatcher.Application.Models.Requests.User;

public class ChangePasswordRequest
{
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}