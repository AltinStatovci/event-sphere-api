using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(u => u.ID);

        builder.Property(u => u.Name).HasMaxLength(30);
        builder.Property(u => u.LastName).HasMaxLength(30);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);

        builder.Property(u => u.Password)
               .IsRequired()
               .HasColumnType("varbinary(64)");

        builder.Property(u => u.Salt)
               .IsRequired()
               .HasColumnType("varbinary(64)");

        builder.HasOne(u => u.Role)
               .WithMany()
               .HasForeignKey(u => u.RoleID);

        builder.Property(u => u.DateCreated)
               .HasDefaultValueSql("GETDATE()");

        builder.HasMany(u => u.Tickets)
               .WithOne(t => t.User)
               .HasForeignKey(t => t.UserID);

        builder.HasMany(u => u.Payments)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserID);
    }
}