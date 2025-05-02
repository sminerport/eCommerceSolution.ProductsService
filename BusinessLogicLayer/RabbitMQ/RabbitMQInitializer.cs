using Microsoft.Extensions.Hosting;

namespace eCommerce.BusinessLogicLayer.RabbitMQ;

public class RabbitMQInitializer : IHostedService
{
    private readonly IRabbitMQPublisher _publisher;

    public RabbitMQInitializer(IRabbitMQPublisher publisher)

    {
        _publisher = publisher;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _publisher.InitializeAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _publisher.DisposeAsync();
    }
}