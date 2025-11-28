using ConversationBackend.Repositories;
using ConversationBackend.Repositories.RepositoryInterfaces;
using ConversationBackend.Services;
using ConversationBackend.Services.RabbitMQ;
using ConversationBackend.Services.ServiceInterfaces;

namespace ConversationBackend.DependencyResolution
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IBusinessPartnerAMLService, BusinessPartnerAMLService>();
            services.AddScoped<IBusinessPartnerAMLRepository, BusinessPartnerAMLRepository>();

            // RabbirMQ Publisher and Consumer
            services.AddScoped<IRabbitPublisher, RabbitPublisher>();
            services.AddHostedService<RabbitConsumer>();
            return services;
        }
    }
}
