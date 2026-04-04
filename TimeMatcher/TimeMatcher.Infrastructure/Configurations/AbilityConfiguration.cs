using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeMatcher.Domain.AbilityAggregate;

namespace TimeMatcher.Infrastructure.Configurations;

public class AbilityConfiguration : IEntityTypeConfiguration<Ability>
{
    public void Configure(EntityTypeBuilder<Ability> builder)
    {
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
