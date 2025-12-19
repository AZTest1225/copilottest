using Microsoft.EntityFrameworkCore;
using PartnerManager.Infrastructure.Models;

namespace PartnerManager.Infrastructure.Data
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
                b.Property(u => u.CreatedAt).HasColumnType("timestamp without time zone");
            });

            modelBuilder.Entity<Partner>(b =>
            {
                b.HasIndex(p => p.Name);
                b.Property(p => p.Name).IsRequired().HasMaxLength(200);
                b.Property(p => p.ContactName).HasMaxLength(200);
                b.Property(p => p.ContactEmail).HasMaxLength(200);
                b.Property(p => p.ContactPhone).HasMaxLength(50);
                b.Property(p => p.Sex).HasMaxLength(20);
                b.Property(p => p.Address).HasMaxLength(300);
                b.Property(p => p.Location).HasMaxLength(200);
                b.Property(p => p.CreatedAt).HasColumnType("timestamp without time zone");
            });

            modelBuilder.Entity<Activity>(b =>
            {
                b.HasIndex(a => a.StartAt);
                b.Property(a => a.Title).IsRequired().HasMaxLength(300);
                b.Property(a => a.StartAt).HasColumnType("timestamp without time zone");
                b.Property(a => a.EndAt).HasColumnType("timestamp without time zone");
                b.Property(a => a.CreatedAt).HasColumnType("timestamp without time zone");
            });

            modelBuilder.Entity<PartnerActivity>(b =>
            {
                b.HasOne(pa => pa.Partner).WithMany(p => p.PartnerActivities).HasForeignKey(pa => pa.PartnerId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(pa => pa.Activity).WithMany(a => a.PartnerActivities).HasForeignKey(pa => pa.ActivityId).OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(pa => new { pa.PartnerId, pa.ActivityId }).IsUnique();
                b.Property(pa => pa.CreatedAt).HasColumnType("timestamp without time zone");
            });
        }
    }
}
