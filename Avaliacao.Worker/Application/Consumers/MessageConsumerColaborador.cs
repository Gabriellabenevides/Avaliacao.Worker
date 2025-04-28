using System.Text;
using Domain.Consumers;
using Domain.Handlers;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqModel = RabbitMQ.Client.IModel;

namespace Application.Consumers;

public class MessageConsumerColaborador : IMessageConsumerColaborador
{
    private readonly IMessageHandlerColaborador _handler;
    private readonly IConnection _connection;
    private readonly RabbitMqModel _channel;

    public MessageConsumerColaborador(IMessageHandlerColaborador handler, IConfiguration configuration)
    {
        _handler = handler;
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"]
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "ColaboradorQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void StartConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await _handler.HandleMessageAsync(message);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: "ColaboradorQueue", autoAck: false, consumer: consumer);
    }
    public void Consume(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await _handler.HandleMessageAsync(message);
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(
            queue: "ColaboradorQueue",
            autoAck: false,
            consumer: consumer);
    }
}
