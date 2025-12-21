using Core.Lib.Domain.Data;
using Core.Lib.Infra;
using Microsoft.EntityFrameworkCore;
using Worker.PriceProcessor;
using Worker.PriceProcessor.Services;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddRabbitMqConfiguration(builder.Configuration);

builder.Services.AddHostedService<PriceWorker>();
builder.Services.AddScoped<MarketHistoryService>();

builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

var host = builder.Build();
host.Run();
