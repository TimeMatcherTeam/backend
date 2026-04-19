using TimeMatcher.Domain.Enums;

namespace TimeMatcher.Domain.MeetingAggregate;

public class Meeting
{
    public Guid Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Name { get; set; }
    public string? Comment { get; set; }
    public string? Link { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyList<MeetingParticipant> MeetingParticipants => meetingParticipants.AsReadOnly();
    private List<MeetingParticipant> meetingParticipants  = [];

    public void AddParticipant(Guid userId, Role role)
    {
        meetingParticipants.Add(new MeetingParticipant
        {
            UserId = userId, 
            MeetingId = Id,
            Role = role
        });
    }

    public void RemoveParticipant(Guid userId)
    {
        var meetingParticipant = meetingParticipants.FirstOrDefault(m => m.UserId == userId);
        meetingParticipants.Remove(meetingParticipant);
    }
}