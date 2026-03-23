using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeMatcher.Domain.GroupAggregate;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Configurations;

public class GroupParticipantConfiguration : IEntityTypeConfiguration<GroupParticipant>
{
    public void Configure(EntityTypeBuilder<GroupParticipant> builder)
    {
        builder
            .Property(p => p.Role)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}