using Microsoft.Extensions.DependencyInjection;
using EventSphere.Domain.Repositories;
using EventSphere.Infrastructure.EntityFramework;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Infrastructure.Repositories;

namespace EventSphere.API
{
    public static class ApplicationInjection
    {
        public static void AddEventSphereServices(this IServiceCollection services)
        {
           
            services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
            services.AddScoped<IEventCategoryService, EventCategoryService>();

          
        }
    }
}
