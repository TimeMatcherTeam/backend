using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(100);
        builder
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasOne(u => u.Calendar)
            .WithOne()
            .HasForeignKey<Calendar>(c => c.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}