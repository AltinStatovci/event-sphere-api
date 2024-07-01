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
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Report");

            builder.HasKey(r => r.ReportId);

            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.UserName).IsRequired();
            builder.Property(r => r.UserEmail).IsRequired();
            builder.Property(r => r.UserLastName).IsRequired();
            builder.Property(r => r.ReportName).IsRequired();
            builder.Property(r => r.ReportDesc).IsRequired();
            builder.Property(r => r.ReportAnsw);
        }
    }
}
