using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ConversationBackend.Services.RabbitMQ
{
    public class RabbitPublisher : IRabbitPublisher
    {
        private readonly ConnectionFactory _factory;

        public RabbitPublisher(IConfiguration config)
        {
            _factory = new ConnectionFactory()
            {
                HostName = config["RabbitMQ:Host"],
                UserName = config["RabbitMQ:User"],
                Password = config["RabbitMQ:Password"]
            };
        }

        public async Task PublishBusinessPartnerRequest(int businessPartnerId)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "businesspartner.request",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(businessPartnerId));

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "businesspartner.request",
                body: body
            );
        }
    }
}
