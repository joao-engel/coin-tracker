using Worker.Audit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<WorkerAudit>();

var host = builder.Build();
host.Run();
