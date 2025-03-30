using Microsoft.EntityFrameworkCore;
using Clean.Domain.Entities;
namespace Clean.Infrastructure.Context;
internal class CleanContext(DbContextOptions<CleanContext> options) : DbContext(options)
{
    internal DbSet<Domain.Entities.Clean> Clean { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Domain.Entities.Clean>(entity =>
        {
            entity.ToTable("Clean");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });
    }
}