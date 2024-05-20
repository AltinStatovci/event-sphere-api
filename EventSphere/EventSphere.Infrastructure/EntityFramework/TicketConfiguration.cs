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
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Ticket");

            builder.HasKey(t => t.ID);

            builder.Property(t => t.TicketType).IsRequired().HasMaxLength(30);
            builder.Property(t => t.Price).IsRequired();
            builder.Property(t => t.DatePurchased).IsRequired();
            builder.Property(t => t.BookingReference).IsRequired().HasMaxLength(100);

            builder.HasOne(t => t.User)
                   .WithMany(u => u.Tickets)
                   .HasForeignKey(t => t.UserID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Event)
                   .WithMany(e => e.Tickets)
                   .HasForeignKey(t => t.EventID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
