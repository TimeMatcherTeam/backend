namespace TimeMatcher.Domain.MeetingAggregate;

public class Meeting
{
    public Guid Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Name { get; set; }
    public string Comment { get; set; }
    public string Link { get; init; }
    public IReadOnlyList<MeetingParticipant> MeetingParticipants => meetingParticipants.AsReadOnly();
    private List<MeetingParticipant> meetingParticipants  = [];
}