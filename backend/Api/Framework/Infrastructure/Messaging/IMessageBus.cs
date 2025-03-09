namespace TalentMesh.Framework.Infrastructure.Messaging
{
    public interface IMessageBus
    {
        /// <summary>
        /// Publishes a message to the specified exchange with the given routing key.
        /// </summary>
        /// <typeparam name="T">Type of the message.</typeparam>
        /// <param name="message">The message to publish.</param>
        /// <param name="exchange">The exchange to publish to.</param>
        /// <param name="routingKey">The routing key to use.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken = default);
    }
}
