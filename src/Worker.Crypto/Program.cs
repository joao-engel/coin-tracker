using Core.Lib.Domain.Data;
using Core.Lib.Infra.Mensageria;
using Core.Lib.Repositories;
using Microsoft.EntityFrameworkCore;
using Worker.Crypto;
using Worker.Crypto.Integration;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddRabbitMqConfiguration(builder.Configuration);
builder.Services.AddHostedService<CryptoWorker>();

builder.Services.AddScoped(typeof(Repository<>));

builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

builder.Services.AddHttpClient<BinanceService>(client =>
{
    client.BaseAddress = new Uri("https://api.binance.com/api/v3/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

var host = builder.Build();
host.Run();
