using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Configurations;

internal class SlotConfiguration: IEntityTypeConfiguration<Slot>
{
    public void Configure(EntityTypeBuilder<Slot> builder)
    {

        builder
            .Property(s => s.StartTime)
            .IsRequired();
        builder
            .Property(s => s.EndTime)
            .IsRequired();

        builder
            .HasOne(s => s.Ability)
            .WithMany()
            .HasForeignKey("AbilityId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(s => s.Meeting)
            .WithMany()
            .HasForeignKey("MeetingId")
            .OnDelete(DeleteBehavior.SetNull);

    }
}