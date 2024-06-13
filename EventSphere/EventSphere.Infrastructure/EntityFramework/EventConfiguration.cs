using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            builder.HasOne(e => e.Location)
                   .WithMany()
                   .HasForeignKey(e => e.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
