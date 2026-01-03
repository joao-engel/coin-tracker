using Core.Lib.Domain.Data;
using Core.Lib.Infra;
using Core.Lib.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Worker.PriceProcessor;
using Worker.PriceProcessor.Services;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddRabbitMqConfiguration(builder.Configuration);

builder.Services.AddHostedService<PriceWorker>();
builder.Services.AddScoped<MarketHistoryService>();

builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection")!;
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
builder.Services.AddScoped<PriceCacheService>();

var host = builder.Build();
host.Run();
