using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Calendar = TimeMatcher.Domain.UserAggregate.Calendar;

namespace TimeMatcher.Infrastructure.Persistence.Configurations;

public class CalendarConfiguration : IEntityTypeConfiguration<Calendar>
{
    public void Configure(EntityTypeBuilder<Calendar> builder)
    {
        builder
           .Navigation(c => c.Slots)
           .HasField("slots")
           .UsePropertyAccessMode(PropertyAccessMode.Field);
        builder
            .HasMany(c => c.Slots)
            .WithOne()
            .HasForeignKey(s => s.CalendarId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}