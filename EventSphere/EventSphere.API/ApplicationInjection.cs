using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Infrastructure.Repositories.Interfaces;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Domain.Entities;

namespace EventSphere.API
{
    public static class ApplicationInjection
    {
        public static void AddEventSphereServices(this IServiceCollection services)
        {
            services.AddScoped<IEventCategoryService, EventCategoryService>();
            services.AddScoped<IGenericRepository<EventCategory>, EventCategoryRepository>();
        }
    }
}
