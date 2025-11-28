using ConversationBackend.Services.ServiceInterfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ConversationBackend.Services.RabbitMQ
{
    public class RabbitConsumer : BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceProvider _serviceProvider;

        public RabbitConsumer(IConfiguration config, IServiceProvider serviceProvider)
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
            var connection = await _factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "businesspartner.response",
                durable: true,
                exclusive: false,
                autoDelete: false
            );
            
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    var businessPartnerId = JsonSerializer.Deserialize<int>(message);

                    using var scope = _serviceProvider.CreateScope();
                    var amlService = scope.ServiceProvider.GetRequiredService<IBusinessPartnerAMLService>();

                    await amlService.GetRefinitivResponse(businessPartnerId);
                    await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message");
                    await channel.BasicNackAsync(eventArgs.DeliveryTag, false, requeue: false);
                }
            };

            await channel.BasicConsumeAsync(
                queue: "businesspartner.request",
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
