using Worker.PriceProcessor;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<PriceWorker>();

var host = builder.Build();
host.Run();
