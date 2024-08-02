using EventSphere.Business.Helper;
using EventSphere.Business.Mappings;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Business.Validator.password;
using EventSphere.Domain.Entities;
using EventSphere.Domain.Repositories;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Infrastructure.Repositories.UserRepository;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Event = EventSphere.Domain.Entities.Event;

namespace EventSphere.API
{
    public static class ApplicationInjection
    {
        public static void AddEventSphereServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IGenericRepository<Ticket>, GenericRepository<Ticket>>();

            services.AddScoped<IGenericRepository<Event>, GenericRepository<Event>>();
            services.AddScoped<IEventService, Business.Services.EventService>();

            services.AddScoped<IGenericRepository<EventCategory>, GenericRepository<EventCategory>>();
            services.AddScoped<IEventCategoryService, EventCategoryService>();
            services.AddScoped<IGenericRepository<Payment>, GenericRepository<Payment>>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IGenericRepository<Report>, GenericRepository<Report>>();
            services.AddScoped<IPromoCodeRepository, PromoCodeRepository>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, Business.Services.AccountService>();

            services.AddScoped<IGenericRepository<Location>, GenericRepository<Location>>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewService, Business.Services.ReviewService>();


            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(Program).Assembly);
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            var mappingConfig = new MappingConfig();
            mappingConfig.Register(config);

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IGenericRepository<Role>, GenericRepository<Role>>();

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPasswordGenerator, PasswordGenerator>();



            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            
           
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ChargeService>();
            
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddSignalR();
            
            services.AddScoped<IPasswordValidator, PasswordValidator>();

            services.AddScoped<IRCEventService, RCEventService>();
            services.AddScoped<IGenericRepository<RCEvent>, GenericRepository<RCEvent>>();

        }
    }
}
