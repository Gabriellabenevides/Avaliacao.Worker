namespace Domain.Consumers;

public interface IMessageConsumerColaborador
{
    void StartConsuming();
    void Consume(CancellationToken cancellationToken);
}
