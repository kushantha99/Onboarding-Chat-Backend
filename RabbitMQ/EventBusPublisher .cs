using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ConversationBackend.RabbitMQ
{
    public class EventBusPublisher : IEventBusPublisher
    {
        private readonly ConnectionFactory _factory;
        //private readonly string _exchange;
        //private readonly string _routingKey;

        public EventBusPublisher(IConfiguration config)
        {
            //_exchange = "onboarding.exchange";
            //_routingKey = "onboarding.provider";

            _factory = new ConnectionFactory()
            {
                HostName = config["RabbitMQ:Host"],
                UserName = config["RabbitMQ:User"],
                Password = config["RabbitMQ:Password"]
            };
        }

        public async Task PublishEvent<T>(T eventData)
        {
            var queueName = typeof(T).Name;

            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );
            var wrapper = new RabbitEventMessage
            {
                EventType = typeof(T).Name,
                Payload = JsonSerializer.Serialize(eventData)
            };
            //await channel.ExchangeDeclareAsync(_exchange, ExchangeType.Direct, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(wrapper));

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                body: body
            );
        }
    }
}
