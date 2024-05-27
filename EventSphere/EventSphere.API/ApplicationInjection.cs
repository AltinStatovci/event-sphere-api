using EventSphere.Business.Helper;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Infrastructure.Repositories.UserRepository;
using Mapster;
using MapsterMapper;

namespace EventSphere.API
{
    public static class ApplicationInjection
    {
        public static void AddEventSphereServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITicketServices, TicketServices>();
            services.AddScoped<IGenericRepository<Ticket>, GenericRepository<Ticket>>();

            services.AddScoped<IGenericRepository<Event>, GenericRepository<Event>>();
            services.AddScoped<IEventService, EventService>();

            services.AddScoped<IGenericRepository<EventCategory>, GenericRepository<EventCategory>>();
            services.AddScoped<IEventCategoryService, EventCategoryService>();
            services.AddScoped<IGenericRepository<Payment>, GenericRepository<Payment>>();
            services.AddScoped<IPaymentService, PaymentServices>();


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(Program).Assembly);
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddScoped<IRoleServices, RoleServices>();
            services.AddScoped<IGenericRepository<Role>, GenericRepository<Role>>();


            services.AddTransient<IEmailService, EmailService>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        }
    }
}
