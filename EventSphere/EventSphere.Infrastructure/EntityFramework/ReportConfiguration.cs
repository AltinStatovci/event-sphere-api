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

            builder.HasKey(r => r.reportId);

            builder.Property(r => r.userId).IsRequired();
            builder.Property(r => r.userName).IsRequired();
            builder.Property(r => r.userEmail).IsRequired();
            builder.Property(r => r.userlastName).IsRequired();
            builder.Property(r => r.reportName).IsRequired();
            builder.Property(r => r.reportDesc).IsRequired();
            builder.Property(r => r.reportAnsw);
        }
    }
}
