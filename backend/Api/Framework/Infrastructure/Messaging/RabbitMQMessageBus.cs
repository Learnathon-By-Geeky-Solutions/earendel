// using System.Text;
// using System.Text.Json;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using System.Diagnostics.CodeAnalysis;

// namespace TalentMesh.Framework.Infrastructure.Messaging
// {
//     [ExcludeFromCodeCoverage]

//     public class RabbitMQMessageBus : IMessageBus, IDisposable
//     {
//         private readonly IConnection _connection;
//         private readonly IModel _channel;
//         private bool _disposed;

//         public RabbitMQMessageBus(IConnectionFactory connectionFactory)
//         {
//             // Create a connection and open a channel
//             _connection = connectionFactory.CreateConnection();
//             _channel = _connection.CreateModel();
//         }

//         public Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken = default)
//         {
//             if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQMessageBus));

//             // Serialize the message to JSON
//             var json = JsonSerializer.Serialize(message);
//             var body = Encoding.UTF8.GetBytes(json);

//             // Ensure the exchange exists (you might want to declare it elsewhere based on your application design)
//             _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true);

//             // Publish the message
//             _channel.BasicPublish(
//                 exchange: exchange,
//                 routingKey: routingKey,
//                 basicProperties: null,
//                 body: body);

//             return Task.CompletedTask;
//         }

//         public void Dispose()
//         {
//             if (!_disposed)
//             {
//                 _channel?.Close();
//                 _connection?.Close();
//                 _disposed = true;
//             }
//         }
//     }
// }



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
            // Create a connection and open a channel
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQMessageBus));

            // Serialize the message to JSON
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            // Ensure the exchange exists
            _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true);

            // Publish the message
            _channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            return Task.CompletedTask;
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    if (_channel != null)
                    {
                        if (_channel.IsOpen)
                        {
                            _channel.Close();
                        }
                        _channel.Dispose();
                    }

                    if (_connection != null)
                    {
                        if (_connection.IsOpen)
                        {
                            _connection.Close();
                        }
                        _connection.Dispose();
                    }
                }
                // Free unmanaged resources (if any) here.

                _disposed = true;
            }
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        // Finalizer to ensure resources are freed if Dispose was not called.
        ~RabbitMQMessageBus()
        {
            Dispose(disposing: false);
        }
    }
}
