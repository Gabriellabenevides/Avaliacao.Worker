namespace Domain.Handlers;

public interface IMessageHandlerColaborador
{
    Task HandleMessageAsync(string message);

}
