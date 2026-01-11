using Api.Analytics.Infra;
using Api.Analytics.Repositories;
using Api.Analytics.Services;
using Core.Lib.Services;
using Npgsql;
using StackExchange.Redis;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

var redisConn = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisConn + ",abortConnect=false"));

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

builder.Services.AddScoped<PriceCacheService>();
builder.Services.AddScoped<DbSession>();
builder.Services.AddScoped<TickerHistoryService>();
builder.Services.AddScoped<TickerHistoryRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.Run();