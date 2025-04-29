using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Messaging
{
    [ExcludeFromCodeCoverage]
    public class SkillsCreatedConsumer : BackgroundService
    {
        private readonly ILogger<SkillsCreatedConsumer> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private IConnection? _connection;
        private IModel? _channel;
        private const string ExchangeName = "skill.events";
        private const string QueueName = "skill.job.created";

        public SkillsCreatedConsumer(ILogger<SkillsCreatedConsumer> logger, IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the exchange and queue, and bind them together
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(QueueName, ExchangeName, "skill.created");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) =>
            {
                _logger.LogInformation("hello..........");
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Received SkillCreated message: {Message}", messageJson);

                // Acknowledge message processing
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
                await Task.CompletedTask;
            };

            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
