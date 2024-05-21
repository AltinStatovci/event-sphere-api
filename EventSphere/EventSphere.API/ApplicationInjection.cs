using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;

using EventSphere.Infrastructure.Repositories;

namespace EventSphere.API
{
    public static class ApplicationInjection
    {
        public static void AddEventSphereServices(this IServiceCollection services)
        {
            services.AddScoped<ITicketServices, TicketServices>();
        }
    }
}
