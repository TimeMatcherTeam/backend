using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Configurations;

public class AbilityConfiguration : IEntityTypeConfiguration<Ability>
{
    public void Configure(EntityTypeBuilder<Ability> builder)
    {
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
