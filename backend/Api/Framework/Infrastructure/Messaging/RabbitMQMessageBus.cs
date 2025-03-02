using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TalentMesh.Framework.Infrastructure.Messaging
{
    public class RabbitMQMessageBus : IMessageBus, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private bool _disposed;

        public RabbitMQMessageBus(IConnectionFactory connectionFactory)
        {
            if (connectionFactory == null) throw new ArgumentNullException(nameof(connectionFactory));

            // Create a connection and open a channel
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQMessageBus));
            if (EqualityComparer<T>.Default.Equals(message, default(T)))
                throw new ArgumentException("Message cannot be null or default.", nameof(message));
            if (string.IsNullOrWhiteSpace(exchange)) throw new ArgumentException("Exchange cannot be null or empty.", nameof(exchange));
            if (string.IsNullOrWhiteSpace(routingKey)) throw new ArgumentException("Routing key cannot be null or empty.", nameof(routingKey));

            // Serialize the message to JSON
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            // Declare the exchange (ensures it exists before publishing)
            _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true);

            // Publish the message
            _channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            return Task.CompletedTask;
        }

        // Proper implementation of IDisposable pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _channel?.Close();
                    _channel?.Dispose();
                    _connection?.Close();
                    _connection?.Dispose();
                }
                _disposed = true;
            }
        }

        ~RabbitMQMessageBus()
        {
            Dispose(false);
        }
    }
}
