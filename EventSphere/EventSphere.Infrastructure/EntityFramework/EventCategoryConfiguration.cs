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
    public class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
    {
        public void Configure(EntityTypeBuilder<EventCategory> builder)
        {
            builder.ToTable("EventCategory");
            builder.HasKey(ec => ec.ID);
            builder.Property(ec => ec.ID)
                   .ValueGeneratedOnAdd();

            builder.Property(ec => ec.CategoryName)
                   .IsRequired()
                   .HasMaxLength(50);
        }
    }
}
