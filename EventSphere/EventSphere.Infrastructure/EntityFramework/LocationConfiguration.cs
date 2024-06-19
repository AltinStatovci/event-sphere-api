using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.EntityFramework
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.City).IsRequired();
            builder.Property(p => p.Country).IsRequired();

        }
    }
}
