using Microsoft.EntityFrameworkCore;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.EntityFramework;
using EventSphere.Infrastructure.EntityFramework.Configurations;

namespace EventSphere.Infrastructure
{
    public class EventSphereDbContext : DbContext
    {
        public EventSphereDbContext(DbContextOptions<EventSphereDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Logg>Logs { get; set; }
        public DbSet<PromoCode>PromoCodes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new EventCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new LogConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());

            modelBuilder.Entity<Role>().HasData(
            new Role { ID = 1, RoleName = "Admin" },
            new Role { ID = 2, RoleName = "Organizer" },
            new Role { ID = 3, RoleName = "User" }
            );

            modelBuilder.Entity<EventCategory>().HasData(
                new EventCategory { ID = 1, CategoryName = "Concerts" },
                new EventCategory { ID = 2, CategoryName = "Sports" },
                new EventCategory { ID = 3, CategoryName = "Outside Activities" }
            );

            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Country = "Kosovo", City = "Prishtina", Latitude = 42.6629, Longitude = 21.1655 },
                new Location { Id = 2, Country = "Kosovo", City = "Mitrovice", Latitude = 42.8914, Longitude = 20.866 },
                new Location {Id = 3, Country = "Kosovo", City = "Pejë", Latitude = 42.2171, Longitude = 20.7436 },
                new Location {Id = 4, Country = "Kosovo", City = "Prizren", Latitude = 42.4635, Longitude = 21.4694 },
                new Location {Id = 5, Country = "Kosovo", City = "Ferizaj", Latitude = 42.3702, Longitude = 21.1483 },
                new Location {Id = 6, Country = "Kosovo", City = "Gjilan", Latitude = 42.6593, Longitude = 20.2887 },
                new Location {Id = 7, Country = "Kosovo", City = "Gjakovë", Latitude = 42.3844, Longitude = 20.4285 }
                );
            modelBuilder.Entity<PromoCode>().HasData(
                new PromoCode { ID = 1, Code = "B3LI3V3R", DiscountPercentage = 20, ExpiryDate = new DateTime(2024, 12, 31), IsValid = true },
                new PromoCode { ID = 2, Code = "FALLSALE", DiscountPercentage = 15, ExpiryDate = new DateTime(2024, 10, 31), IsValid = true },
                new PromoCode { ID = 3, Code = "SUMMERFUN", DiscountPercentage = 10, ExpiryDate = new DateTime(2024, 8, 31), IsValid = true },
                new PromoCode { ID = 4, Code = "SPRINGSALE", DiscountPercentage = 15, ExpiryDate = new DateTime(2024, 5, 31), IsValid = true },
                new PromoCode { ID = 5, Code = "WINTERWONDER", DiscountPercentage = 25, ExpiryDate = new DateTime(2024, 12, 21), IsValid = true }

            );

        }
    }
}
