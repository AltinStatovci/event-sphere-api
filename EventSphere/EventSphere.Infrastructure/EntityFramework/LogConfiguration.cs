using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Serilog;

namespace EventSphere.Infrastructure.EntityFramework
{
    public class LogConfiguration : IEntityTypeConfiguration<Logg>
    {
        public void Configure(EntityTypeBuilder<Logg> builder)
        {
            builder.ToTable("Logs");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Message);
            builder.Property(l => l.MessageTemplate);
            builder.Property(l => l.TimeStamp);
            builder.Property(l => l.Level);
            builder.Property(l => l.Exception);
            builder.Property(l => l.Properties);
        }
    }
}