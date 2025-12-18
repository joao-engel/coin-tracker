using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Core.Lib.Infra;
public static class RabbitMQConfiguration
{
    public static IServiceCollection AddRabbitMqConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(sp =>
        {
            var connectionString = configuration["ConnectionStrings:RabbitMQConnection"]
                ?? throw new Exception("A connection string do RabbitMQ não foi encontrada.");

            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };

            return factory.CreateConnectionAsync().Result;
        });

        services.AddSingleton<RabbitMQService>();
        return services;
    }
}
