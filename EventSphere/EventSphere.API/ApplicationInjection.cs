using EventSphere.Business.Helper;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;

namespace EventSphere.API
{
    public static class ApplicationInjection
    {
        public static void AddEventSphereServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITicketServices, TicketServices>();
            services.AddScoped<IGenericRepository<Ticket>, GenericRepository<Ticket>>();
            services.AddScoped<IGenericRepository<EventCategory>, GenericRepository<EventCategory>>();
            services.AddScoped<IEventCategoryService, EventCategoryService>();

            services.AddTransient<IEmailService, EmailService>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        }
    }
}
