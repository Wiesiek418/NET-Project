namespace Infrastructure.Mqtt;

public interface IMqttMessageHandler<TMessage>
    where TMessage : class
{
    Task HandleAsync(TMessage payload, CancellationToken ct);
}