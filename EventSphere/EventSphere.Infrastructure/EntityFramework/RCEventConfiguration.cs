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
    public class RCEventConfiguration : IEntityTypeConfiguration<RCEvent>
    {

        public void Configure(EntityTypeBuilder<RCEvent> builder)
        {
            builder.ToTable("RCEvent");
            builder.HasKey(rc=>rc.Id);
            builder.Property(rc => rc.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(rc => rc.UserId)
                   .IsRequired();
            builder.Property(rc => rc.EventId)
                   .IsRequired();
            builder.Property(rc => rc.Ecount)
                   .IsRequired();

        }
    }
    
}
