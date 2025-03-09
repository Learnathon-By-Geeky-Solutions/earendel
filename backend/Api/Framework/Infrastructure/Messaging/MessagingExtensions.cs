using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Framework.Infrastructure.Messaging
{
    public static class MessagingExtensions
    {
        public static IServiceCollection ConfigureRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind RabbitMQ settings
            services.Configure<RabbitMQOptions>(configuration.GetSection(nameof(RabbitMQOptions)));

            // Register the RabbitMQ connection factory as a singleton
            services.AddSingleton<IConnectionFactory>(sp =>
            {
                var rabbitOptions = sp.GetRequiredService<IOptions<RabbitMQOptions>>().Value
                    ?? throw new TalentMeshException("RabbitMQ options cannot be null");

                return new ConnectionFactory
                {
                    HostName = rabbitOptions.HostName,
                    UserName = rabbitOptions.UserName,
                    Password = rabbitOptions.Password,
                    VirtualHost = rabbitOptions.VirtualHost,
                    Port = rabbitOptions.Port,
                    DispatchConsumersAsync = true
                };
            });

            // Register your message bus (this is your abstraction over RabbitMQ operations)
            services.AddSingleton<IMessageBus, RabbitMQMessageBus>(); // You'll need to implement RabbitMQMessageBus

            return services;
        }
    }
}
