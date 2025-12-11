using ConversationBackend.RabbitMQ;
using ConversationBackend.RabbitMQ.Events;
using ConversationBackend.Repositories;
using ConversationBackend.Repositories.RepositoryInterfaces;
using ConversationBackend.Services;
using ConversationBackend.Services.ServiceInterfaces;

namespace ConversationBackend.DependencyResolution
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IBusinessPartnerAMLService, BusinessPartnerAMLService>();
            services.AddScoped<IBusinessPartnerAMLRepository, BusinessPartnerAMLRepository>();
            services.AddScoped<IAMLRepository, AMLRepository>();

            services.AddScoped<IDRPService, DRPService>();
            services.AddScoped<IDRPSystemRepository, DRPSystemRepository>();

            services.AddScoped<IEventBusPublisher, EventBusPublisher>();
            services.AddHostedService<EventBusConsumer>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(BusinessPartnerRequestCreatedEvent).Assembly);
            });

            return services;
        }
    }
}
