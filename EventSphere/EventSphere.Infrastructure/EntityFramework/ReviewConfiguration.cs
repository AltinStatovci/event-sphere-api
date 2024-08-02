using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventSphere.Infrastructure.EntityFramework.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Review");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReviewerId).IsRequired();
            builder.Property(r => r.OrganizerId).IsRequired();
            builder.Property(r => r.Rating)
                   .HasConversion<int>()
                   .IsRequired();
            builder.Property(r => r.ReviewText)
                   .HasMaxLength(1000)
                   .IsRequired();
            builder.Property(r => r.CreatedAt).IsRequired();

            builder.HasOne(r => r.Reviewer)
                   .WithMany(u => u.Reviews)
                   .HasForeignKey(r => r.ReviewerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Organizer)
                   .WithMany()
                   .HasForeignKey(r => r.OrganizerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
