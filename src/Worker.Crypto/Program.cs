using Core.Lib.Infra;
using Worker.Crypto;
using Worker.Crypto.Integration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddRabbitMqConfiguration(builder.Configuration);
builder.Services.AddHostedService<CryptoWorker>();

builder.Services.AddHttpClient<BinanceService>(client =>
{
    client.BaseAddress = new Uri("https://api.binance.com/api/v3/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

var host = builder.Build();
host.Run();
