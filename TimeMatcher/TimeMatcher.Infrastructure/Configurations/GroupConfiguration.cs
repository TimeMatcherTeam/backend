using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeMatcher.Domain.GroupAggregate;

namespace TimeMatcher.Infrastructure.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder
           .Navigation(g => g.GroupParticipants)
           .HasField("groupParticipants")
           .UsePropertyAccessMode(PropertyAccessMode.Field);
        builder
            .HasMany(g => g.GroupParticipants)
            .WithOne()
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}