using Domain.Consumers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Avaliacao.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageConsumerColaborador _messageConsumer;

        public Worker(ILogger<Worker> logger, IMessageConsumerColaborador messageConsumer)
        {
            _logger = logger;
            _messageConsumer = messageConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            try
            {
                // Verifica se o token de cancelamento foi solicitado antes de iniciar
                stoppingToken.ThrowIfCancellationRequested();

                // Começa a consumir as mensagens do RabbitMQ
                await Task.Run(() => _messageConsumer.Consume(stoppingToken), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Worker execution was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming messages.");
            }
            finally
            {
                _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
            }
        }
    }
}
