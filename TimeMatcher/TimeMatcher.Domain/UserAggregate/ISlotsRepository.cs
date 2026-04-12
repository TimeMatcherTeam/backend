namespace TimeMatcher.Domain.UserAggregate;

public interface ISlotsRepository: IRepository
{
    Task<Slot?> GetById(Guid slotId);
    Task<Slot[]> GetFilteredByDateTimeSlots(Guid calendarId, DateTime start, DateTime end);
    Task<Slot> Create(Slot slot);
    void Delete(Slot slot);
}