using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TalentMesh.Framework.Infrastructure.Messaging
{
    public abstract class RabbitMqConsumerBase : BackgroundService
    {
        protected readonly ILogger _logger;
        protected readonly IConnectionFactory _connectionFactory;
        protected IConnection? _connection;
        protected IModel? _channel;
        protected readonly string _exchangeName;
        protected readonly string _queueName;
        protected readonly string _routingKey;

        protected RabbitMqConsumerBase(
            ILogger logger,
            IConnectionFactory connectionFactory,
            string exchangeName,
            string queueName,
            string routingKey)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _exchangeName = exchangeName;
            _queueName = queueName;
            _routingKey = routingKey;
        }

        protected void SetUpRabbitMQConnection()
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare exchange, queue and bind
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(_queueName, _exchangeName, _routingKey);

            // Set prefetch count
            _channel.BasicQos(0, 10, false);
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
