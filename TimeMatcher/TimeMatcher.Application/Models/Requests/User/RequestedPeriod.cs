namespace TimeMatcher.Application.Models.Requests.User;

public class RequestedPeriod
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }//todo here shoud be checked for start < end and lets dont do it in the managers
}