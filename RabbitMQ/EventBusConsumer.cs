using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ConversationBackend.RabbitMQ
{
    public class EventBusConsumer: BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceProvider _serviceProvider;

        public EventBusConsumer(IConfiguration config, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _factory = new ConnectionFactory()
            {
                HostName = config["RabbitMQ:Host"],
                UserName = config["RabbitMQ:User"],
                Password = config["RabbitMQ:Password"]
            };
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var eventTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(INotification).IsAssignableFrom(t) && t.IsClass)
                .ToList();

            foreach (var eventType in eventTypes)
            {
                var queueName = eventType.Name;

                // Start a consumer per event type
                _ = Task.Run(() => StartConsumerFor(queueName, eventType, stoppingToken));
            }
        }

        private async Task StartConsumerFor(string queueName, Type eventType, CancellationToken ct)
        {
            var connection = await _factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queueName, true, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var wrapper = JsonSerializer.Deserialize<RabbitEventMessage>(json);

                    var eventObj = JsonSerializer.Deserialize(wrapper.Payload, eventType);

                    using var scope = _serviceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Publish(eventObj, ct);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch
                {
                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                }
            };

            await channel.BasicConsumeAsync(queueName, false, consumer);
        }

    }
}
