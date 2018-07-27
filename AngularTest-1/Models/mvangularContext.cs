using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AngularTest.Models
{
    public partial class mvangularContext : DbContext
    {
        public mvangularContext()
        {
        }

        public mvangularContext(DbContextOptions<mvangularContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FeatureFunding> FeatureFunding { get; set; }
        public virtual DbSet<FeatureFundinginterest> FeatureFundinginterest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeatureFunding>(entity =>
            {
                entity.Property(e => e.EstimatedDeliveryDate).HasColumnType("smalldatetime");

                entity.Property(e => e.FundingEndsAt).HasColumnType("smalldatetime");

                entity.Property(e => e.LongDescription).IsRequired();

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.TargetAmount).HasColumnType("decimal(5, 0)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(2000);
            });

            modelBuilder.Entity<FeatureFundinginterest>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(5, 0)");

                entity.Property(e => e.Created).HasColumnType("smalldatetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(1000);
            });
        }
    }
}
