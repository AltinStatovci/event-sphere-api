using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(u => u.Id);

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
               .HasForeignKey(u => u.RoleId);

        builder.Property(u => u.DateCreated)
               .HasDefaultValueSql("GETDATE()");

       

        builder.HasMany(u => u.Payments)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId);
    }
}