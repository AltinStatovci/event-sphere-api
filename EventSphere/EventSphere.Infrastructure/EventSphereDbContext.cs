using Microsoft.EntityFrameworkCore;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.EntityFramework;

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
                new Location { Id = 1, Country = "Kosovo", City = "Prishtina" },
                new Location { Id = 2, Country = "Kosovo", City = "Mitrovica" },
                new Location { Id = 3, Country = "Kosovo", City = "Pejë" },
                new Location { Id = 4, Country = "Kosovo", City = "Prizren" },
                new Location { Id = 5, Country = "Kosovo", City = "Ferizaj" },
                new Location { Id = 6, Country = "Kosovo", City = "Gjilan" },
                new Location { Id = 7, Country = "Kosovo", City = "Gjakovë" }
                );

        }
    }
}
