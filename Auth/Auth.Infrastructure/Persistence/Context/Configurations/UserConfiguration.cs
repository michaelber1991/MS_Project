using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(255);
        builder.HasIndex(r => r.Username).IsUnique();
        builder.Property(u => u.GlobalName).IsRequired().HasMaxLength(255);
        builder.HasIndex(r => r.GlobalName).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(r => r.Email).IsUnique();
        builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
        ;
    }
}