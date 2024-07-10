using EventSphere.Business.Helper;
using EventSphere.Business.Mappings;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Infrastructure.Repositories.UserRepository;
using Mapster;
using MapsterMapper;
using Stripe;
using Event = EventSphere.Domain.Entities.Event;

namespace EventSphere.API
{
    public static class ApplicationInjection
    {
        public static void AddEventSphereServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITicketServices, TicketServices>();
            services.AddScoped<IGenericRepository<Ticket>, GenericRepository<Ticket>>();

            services.AddScoped<IGenericRepository<Event>, GenericRepository<Event>>();
            services.AddScoped<IEventServices, EventServices>();

            services.AddScoped<IGenericRepository<EventCategory>, GenericRepository<EventCategory>>();
            services.AddScoped<IEventCategoryServices, EventCategoryServices>();
            services.AddScoped<IGenericRepository<Payment>, GenericRepository<Payment>>();
            services.AddScoped<IPaymentServices, PaymentServices>();
            services.AddScoped<IReportServices, ReportServices>();
            services.AddScoped<IGenericRepository<Report>, GenericRepository<Report>>();
            services.AddScoped<IPromoCodeRepository, PromoCodeRepository>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IAccountServices, AccountServices>();

            services.AddScoped<IGenericRepository<Location>, GenericRepository<Location>>();
            services.AddScoped<ILocationServices, LocationServices>();

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(Program).Assembly);
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            var mappingConfig = new MappingConfig();
            mappingConfig.Register(config);

            services.AddScoped<IRoleServices, RoleServices>();
            services.AddScoped<IGenericRepository<Role>, GenericRepository<Role>>();

            services.AddTransient<IEmailServices, EmailServices>();



            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            
           
            services.AddScoped<ILogServices, LogServices>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ChargeService>();

        }
    }
}
