using Microsoft.EntityFrameworkCore;
using PartnerManager.Api.Models;

namespace PartnerManager.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Partner> Partners { get; set; } = null!;
        public DbSet<Activity> Activities { get; set; } = null!;
        public DbSet<PartnerActivity> PartnerActivities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(u => u.Email).IsUnique();
                b.Property(u => u.UserName).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Partner>(b =>
            {
                b.HasIndex(p => p.Name);
                b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<Activity>(b =>
            {
                b.HasIndex(a => a.StartAt);
                b.Property(a => a.Title).IsRequired().HasMaxLength(300);
            });

            modelBuilder.Entity<PartnerActivity>(b =>
            {
                b.HasOne(pa => pa.Partner).WithMany(p => p.PartnerActivities).HasForeignKey(pa => pa.PartnerId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(pa => pa.Activity).WithMany(a => a.PartnerActivities).HasForeignKey(pa => pa.ActivityId).OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(pa => new { pa.PartnerId, pa.ActivityId }).IsUnique();
            });
        }
    }
}
