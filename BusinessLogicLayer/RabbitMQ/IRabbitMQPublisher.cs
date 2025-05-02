namespace eCommerce.BusinessLogicLayer.RabbitMQ;

public interface IRabbitMQPublisher
{
    /// <summary>
    /// Publish a message to RabbitMQ.
    /// </summary>
    /// <typeparam name="T">The type of the message.</typeparam>
    /// <param name="routingKey">The routing key to use for the message.</param>
    /// <param name="message">The message to publish.</param>
    /// <returns>Task representing the publish operation.</returns>
    Task Publish<T>(string routingKey, T message);

    /// <summary>
    /// Publish a message to RabbitMQ with headers.
    /// </summary>
    /// <typeparam name="T">The type of the message.</typeparam>
    /// <param name="headers">The headers to include in the message.</param>
    /// <param name="message">The message to publish.</param>
    /// <returns>Task representing the publish operation.</returns>
    Task Publish<T>(Dictionary<string, object?> headers, T message);

    /// <summary>
    /// Initialize the RabbitMQ connection and channel.
    /// </summary>
    /// <returns>Task representing the initialization operation.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Dispose the RabbitMQ connection and channel asynchronously.
    /// </summary>
    /// <returns>ValueTask representing the dispose operation.</returns>
    ValueTask DisposeAsync();
}