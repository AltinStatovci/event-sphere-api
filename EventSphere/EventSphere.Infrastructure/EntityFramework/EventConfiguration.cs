using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventSphere.Infrastructure.EntityFramework
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Event");

            builder.HasKey(e => e.ID);

            builder.Property(e => e.EventName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(300);
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();
            builder.Property(e => e.PhotoData);
            builder.Property(e => e.MaxAttendance).IsRequired();
            builder.Property(e => e.AvailableTickets).IsRequired();
            builder.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");

            builder.HasOne(e => e.Category)
                   .WithMany()
                   .HasForeignKey(e => e.CategoryID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Organizer)
                   .WithMany()
                   .HasForeignKey(e => e.OrganizerID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Location)
                .WithOne()
                .HasForeignKey<Event>(e => e.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
