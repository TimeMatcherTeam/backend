using TimeMatcher.Domain.GroupAggregate;

namespace TimeMatcher.Application.Responses.Group;

public record GroupResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required GroupParticipantResponse[] Participants { get; init; }
};