using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TalentMesh.Framework.Infrastructure.SignalR;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Framework.Infrastructure.Common;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Messaging
{
    [ExcludeFromCodeCoverage]
    public abstract class RabbitMqConsumer<TMessage> : RabbitMqConsumerBase
    {
        protected readonly IHubContext<NotificationHub> _hubContext;
        protected readonly IServiceScopeFactory _scopeFactory;

        protected RabbitMqConsumer(
         ILogger logger,
         IConnectionFactory connectionFactory,
         string exchangeName,
         string queueName,
         string routingKey,
         IHubContext<NotificationHub> hubContext,
         IServiceScopeFactory scopeFactory)
         : base(logger, connectionFactory, exchangeName, queueName, routingKey) // Parameters match base constructor order
        {
            _hubContext = hubContext;
            _scopeFactory = scopeFactory;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetUpRabbitMQConnection();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) => await ProcessMessageAsync(ea, stoppingToken);
            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Received event message.");

            try
            {
                var messageJson = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonHelper.Deserialize<TMessage>(messageJson);

                if (message is null)
                {
                    _logger.LogWarning("Failed to deserialize message or message was null.");
                    if (_channel != null)
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                await ProcessDomainMessage(message, scope, stoppingToken);

                // Build and send final response via SignalR
                var finalResponse = CreateFinalResponse(message);
                if (finalResponse is not null)
                {
                    var finalJson = JsonHelper.Serialize(finalResponse);
                    _logger.LogInformation("Final Response: {Response}", finalJson);

                    var requestedBy = GetRequestedBy(message);
                    var requestedByStr = requestedBy.ToString();

                    if (!string.IsNullOrEmpty(requestedByStr))
                    {
                        await _hubContext.Clients
                            .Group($"user:{requestedByStr}")
                            .SendAsync("ReceiveMessage", finalJson, cancellationToken: stoppingToken);
                    }

                }

                if (_channel != null)
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                if (_channel != null)
                {
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            }
        }

        // This method must be implemented by the derived class to perform domain-specific work.
        protected abstract Task ProcessDomainMessage(TMessage message, IServiceScope scope, CancellationToken stoppingToken);

        // Override to customize the final response object.
        protected virtual object CreateFinalResponse(TMessage message)
        {
            return message ?? new object(); // or return a default response instance
        }

        // Extract the requesting user's Id from the message.
        protected abstract Guid GetRequestedBy(TMessage message);
    }
}
