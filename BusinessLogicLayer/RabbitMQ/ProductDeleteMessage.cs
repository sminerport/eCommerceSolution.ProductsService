namespace eCommerce.BusinessLogicLayer.RabbitMQ;

public record ProductDeleteMessage(Guid ProductID, string? ProductName);