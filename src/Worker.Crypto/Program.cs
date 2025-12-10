using Worker.Crypto;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<CryptoWorker>();

var host = builder.Build();
host.Run();
