using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeMatcher.Domain.MeetingAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Configurations;

public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder
            .Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder
            .Property(m => m.Comment)
            .HasMaxLength(3000);
        builder
            .Property(m => m.Link)
            .HasMaxLength(500);
        builder
            .Property(m => m.StartTime)
            .IsRequired();
        builder
            .Property(m => m.EndTime)
            .IsRequired();

        builder
           .Navigation(g => g.MeetingParticipants)
           .HasField("meetingParticipants")
           .UsePropertyAccessMode(PropertyAccessMode.Field);
        builder
            .HasMany(m => m.MeetingParticipants)
            .WithOne()
            .HasForeignKey(p => p.MeetingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}