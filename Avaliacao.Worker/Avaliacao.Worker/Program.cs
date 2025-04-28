using Application.Consumers;
using Application.Handlers;
using Avaliacao.Worker;
using Domain.Consumers;
using Domain.Handlers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // HttpClient para requisições HTTP
        //services.AddHttpClient<IHttpRequestService, HttpRequestService>(client =>
        //{
        //    client.Timeout = TimeSpan.FromSeconds(30);
        //    // client.BaseAddress = new Uri("https://sua-api.com/"); // opcional
        //});

        // Configurações do RabbitMQ
        services.AddSingleton<IMessageHandlerColaborador, MessageHandlerColaborador>();
        services.AddSingleton<IMessageConsumerColaborador, MessageConsumerColaborador>();

        // Registrando o Worker
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
