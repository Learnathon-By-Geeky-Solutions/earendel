using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Messaging
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQMessageBus : IMessageBus, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private bool _disposed;

        public RabbitMQMessageBus(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQMessageBus));

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true);
            _channel.BasicPublish(exchange, routingKey, null, body);

            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                CloseAndDispose(_channel);
                CloseAndDispose(_connection);
            }

            _disposed = true;
        }

        private static void CloseAndDispose(IDisposable? resource)
        {
            if (resource is IModel channel && channel.IsOpen) channel.Close();
            if (resource is IConnection connection && connection.IsOpen) connection.Close();
            resource?.Dispose();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~RabbitMQMessageBus()
        {
            Dispose(disposing: false);
        }
    }
}
