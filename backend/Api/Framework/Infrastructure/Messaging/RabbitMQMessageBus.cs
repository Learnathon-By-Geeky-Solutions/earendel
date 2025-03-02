using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TalentMesh.Framework.Infrastructure.Messaging
{
    public class RabbitMQMessageBus : IMessageBus, IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private bool _disposed;

        public RabbitMQMessageBus(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            // Create a connection and open a channel
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQMessageBus));

            // Serialize the message to JSON
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            // Ensure the exchange exists (you might want to declare it elsewhere based on your application design)
            _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true);

            // Publish the message
            _channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _channel?.Close();
                _connection?.Close();
                _disposed = true;
            }
        }
    }
}
