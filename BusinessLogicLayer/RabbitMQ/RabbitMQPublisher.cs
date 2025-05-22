using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using RabbitMQ.Client;

namespace eCommerce.BusinessLogicLayer.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher, IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _disposed;

    public async Task InitializeAsync()
    {
        ConnectionFactory connectionFactory = new ConnectionFactory
        {
            HostName = _configuration["RABBITMQ_HOST"]!,
            UserName = _configuration["RABBITMQ_USERNAME"]!,
            Password = _configuration["RABBITMQ_PASSWORD"]!,
            Port = Convert.ToInt32(_configuration["RABBITMQ_PORT"]!)
        };

        _connection = await connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Publish<T>(string routingKey, T message)
    {
        // Serialize the message to JSON
        string messageJson = JsonSerializer.Serialize(message);

        // Convert the JSON string to a byte array using UTF8 encoding
        byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

        string exchangeName = _configuration["RABBITMQ_PRODUCTS_EXCHANGE"]!;

        if (_channel != null)
        {
            await _channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true);

            BasicProperties properties = new BasicProperties();

            // Specify the type argument explicitly for BasicPublishAsync
            await _channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: messageBodyInBytes);
        }
    }

    public async Task Publish<T>(Dictionary<string, object?> headers, T message)
    {
        // Serialize the message to JSON
        string messageJson = JsonSerializer.Serialize(message);

        // Convert the JSON string to a byte array using UTF8 encoding
        byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

        string exchangeName = _configuration["RABBITMQ_PRODUCTS_EXCHANGE"]!;

        if (_channel != null)
        {
            await _channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Headers,
                durable: true);

            BasicProperties properties = new BasicProperties()
            {
                Headers = headers
            };

            // Specify the type argument explicitly for BasicPublishAsync
            await _channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: string.Empty,
                mandatory: true,
                basicProperties: properties,
                body: messageBodyInBytes);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;
        _disposed = true;

        if (_channel is not null)
        {
            // gracefully close the channel
            try
            { await _channel.CloseAsync(); }
            catch { /* TODO: log if needed */ }

            if (_channel is IAsyncDisposable asyncChannel)
            {
                await asyncChannel.DisposeAsync();
            }
            else
            {
                _channel.Dispose();
            }
        }

        if (_connection is not null)
        {
            // gracefully close the connection
            try
            { await _connection.CloseAsync(); }
            catch { /* TODO: log if needed */ }
            if (_connection is IAsyncDisposable asyncConnection)
            {
                await asyncConnection.DisposeAsync();
            }
            else
            {
                _connection.Dispose();
            }
        }
    }
}