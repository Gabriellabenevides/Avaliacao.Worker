using System.Text.Json;
using Domain.Handlers;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Handlers;

public class MessageHandlerColaborador : IMessageHandlerColaborador
{
    //private readonly IHttpRequestService _httpService;
    private readonly ILogger<MessageHandlerColaborador> _logger;

    public MessageHandlerColaborador(ILogger<MessageHandlerColaborador> logger)
    {
        _logger = logger;
    }

    public async Task HandleMessageAsync(string message)
    {
        try
        {
            var model = JsonSerializer.Deserialize<ColaboradorMessage>(message);
            //await _httpService.SendRequestAsync(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a mensagem");
            throw;
        }
    }
}
