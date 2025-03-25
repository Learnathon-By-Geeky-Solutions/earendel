using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Job.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using TalentMesh.Framework.Infrastructure.SignalR;

namespace TalentMesh.Module.Job.Infrastructure.Messaging
{
    public class HrJobListConsumer : BackgroundService
    {
        private readonly ILogger<HrJobListConsumer> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<NotificationHub> _hubContext;

        private IConnection? _connection;
        private IModel? _channel;
        private const string ExchangeName = "hr.job.list.events";
        private const string QueueName = "hr.job.list.fetched";

        public HrJobListConsumer(
            ILogger<HrJobListConsumer> logger,
            IConnectionFactory connectionFactory,
            IHubContext<NotificationHub> hubContext,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetUpRabbitMQConnection();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) => await ProcessMessageAsync(ea, stoppingToken);

            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private void SetUpRabbitMQConnection()
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(QueueName, ExchangeName, "hr.job.list.fetched");

            // Prefetch messages to avoid overloading a single consumer
            _channel.BasicQos(0, 10, false);
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Received HR job list event message.");

                var messageJson = Encoding.UTF8.GetString(ea.Body.ToArray());
                var hrMessage = DeserializeMessage(messageJson);

                if (hrMessage == null)
                {
                    _logger.LogWarning("Failed to deserialize HR job list message.");
                    _channel.BasicNack(ea.DeliveryTag, false, true); // Requeue message
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();

                var hrIds = hrMessage.HRs.Select(hr => hr.Id).ToList();

                var allJobs = await dbContext.Jobs
                    .Where(j => hrIds.Contains(j.CreatedBy))
                    .Select(j => new JobItem
                    {
                        Id = j.Id,
                        Title = j.Name,
                        Description = j.Description,
                        CreatedAt = j.Created.UtcDateTime,
                        PostedBy = j.CreatedBy
                    })
                    .ToListAsync(stoppingToken);

                var jobsByHr = allJobs.ToLookup(j => j.PostedBy);

                foreach (var hr in hrMessage.HRs)
                {
                    hr.Jobs = jobsByHr[hr.Id].ToList();
                    hr.JobCount = hr.Jobs.Count;
                }

                // If the original sort was by job count, sort the HR list accordingly
                if (!string.IsNullOrEmpty(hrMessage.SortBy) &&
                    hrMessage.SortBy.Equals("jobcount", StringComparison.OrdinalIgnoreCase))
                {
                    bool isAscending = string.Equals(hrMessage.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                    hrMessage.HRs = isAscending
                        ? hrMessage.HRs.OrderBy(hr => hr.JobCount).ToList()
                        : hrMessage.HRs.OrderByDescending(hr => hr.JobCount).ToList();
                }

                var finalResponse = new
                {
                    EventType = "hr.job.list.updated",
                    Timestamp = DateTime.UtcNow,
                    TotalHrCount = hrMessage.TotalRecords,
                    TotalJobCount = allJobs.Count,
                    TotalRecords = hrMessage.TotalRecords,
                    HRs = hrMessage.HRs
                };

                var finalJson = JsonSerializer.Serialize(finalResponse);
                _logger.LogInformation("Final HR Job List Response: {Response}", finalJson);
                await _hubContext.Clients.Group($"user:{hrMessage.RequestedBy}").SendAsync("ReceiveMessage", finalJson);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error processing HR job list message: {Error}", ex.Message);
                _channel.BasicNack(ea.DeliveryTag, false, true); // Requeue message
            }
        }

        private HrMessage? DeserializeMessage(string messageJson)
        {
            try
            {
                return JsonSerializer.Deserialize<HrMessage>(messageJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deserializing HR job list message: {Error}", ex.Message);
                return null;
            }
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }

    public class HrMessage
    {
        public List<HrItem> HRs { get; set; } = new List<HrItem>();
        public Guid RequestedBy { get; set; }
        public string? SortBy { get; set; }  // Passed from GetHrsAsync event payload
        public string? SortDirection { get; set; }  // Passed from GetHrsAsync event payload
        public int TotalRecords { get; set; } 
    }

    public class HrItem
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public int JobCount { get; set; }
        public List<JobItem> Jobs { get; set; } = new List<JobItem>();
    }

    public class JobItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public Guid PostedBy { get; set; }
    }
}
