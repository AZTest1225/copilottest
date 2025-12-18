using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PartnerManager.Infrastructure.Migrations
{
    [DbContext(typeof(PartnerManager.Infrastructure.Data.AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("PartnerManager.Infrastructure.Models.Activity", b =>
            {
                b.Property<int>("Id");

                b.Property<int>("Status");

                b.Property<DateTime>("CreatedAt");

                b.Property<string>("Description").HasColumnType("text");

                b.Property<DateTime>("EndAt");

                b.Property<string>("Title").IsRequired().HasMaxLength(300);

                b.Property<DateTime>("StartAt");

                b.HasKey("Id");

                b.HasIndex("StartAt");

                b.ToTable("Activities");
            });

            modelBuilder.Entity("PartnerManager.Infrastructure.Models.Partner", b =>
            {
                b.Property<int>("Id");

                b.Property<DateTime>("CreatedAt");

                b.Property<string>("ContactEmail").HasMaxLength(200);

                b.Property<string>("ContactName").HasMaxLength(200);

                b.Property<string>("ContactPhone").HasMaxLength(50);

                b.Property<int>("Status");

                b.Property<string>("Name").IsRequired().HasMaxLength(200);

                b.HasKey("Id");

                b.HasIndex("Name");

                b.ToTable("Partners");
            });

            modelBuilder.Entity("PartnerManager.Infrastructure.Models.PartnerActivity", b =>
            {
                b.Property<int>("Id");

                b.Property<int>("ActivityId");

                b.Property<DateTime>("CreatedAt");

                b.Property<int>("PartnerId");

                b.Property<string>("Role").HasMaxLength(200);

                b.HasKey("Id");

                b.HasIndex("PartnerId", "ActivityId").IsUnique();

                b.HasIndex("ActivityId");

                b.ToTable("PartnerActivities");
            });

            modelBuilder.Entity("PartnerManager.Infrastructure.Models.User", b =>
            {
                b.Property<int>("Id");

                b.Property<DateTime>("CreatedAt");

                b.Property<bool>("IsActive");

                b.Property<string>("PasswordHash").IsRequired();

                b.Property<int>("Role");

                b.Property<string>("UserName").IsRequired().HasMaxLength(100);

                b.Property<string>("Email").IsRequired().HasMaxLength(200);

                b.HasKey("Id");

                b.HasIndex("Email").IsUnique();

                b.ToTable("Users");
            });

            modelBuilder.Entity("PartnerManager.Infrastructure.Models.PartnerActivity", b =>
            {
                b.HasOne("PartnerManager.Infrastructure.Models.Activity", "Activity")
                    .WithMany("PartnerActivities")
                    .HasForeignKey("ActivityId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("PartnerManager.Infrastructure.Models.Partner", "Partner")
                    .WithMany("PartnerActivities")
                    .HasForeignKey("PartnerId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
