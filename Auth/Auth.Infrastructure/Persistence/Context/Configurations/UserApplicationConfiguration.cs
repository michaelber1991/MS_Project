using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.Context.Configurations;

public class UserApplicationConfiguration : IEntityTypeConfiguration<UserApplication>
{
    public void Configure(EntityTypeBuilder<UserApplication> builder)
    {
        builder.HasKey(ua => new { ua.UserId, ua.ApplicationId });
        builder.Property(ua => ua.CreatedAt).HasDefaultValueSql("GETDATE()");

        builder.HasOne(ua => ua.User)
            .WithMany(u => u.UserApplications)
            .HasForeignKey(ua => ua.UserId);

        builder.HasOne(ua => ua.Application)
            .WithMany(a => a.UserApplications)
            .HasForeignKey(ua => ua.ApplicationId);
    }
}