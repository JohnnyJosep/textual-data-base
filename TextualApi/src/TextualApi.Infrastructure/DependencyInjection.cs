using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using RabbitMQ.Client;
using TextualApi.Application.Interfaces;
using TextualApi.Infrastructure.Services.IndexService;

namespace TextualApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticConnectionString = configuration.GetConnectionString("Elasticsearch");
            var elasticUri = new Uri(elasticConnectionString);
            var settings = new ConnectionSettings(elasticUri);
            var client = new ElasticClient(settings);
            services.AddTransient(sp => client);
            
            var rabbitMqConnectionString = configuration.GetConnectionString("RabbitMQ");
            var factory = new ConnectionFactory {HostName = rabbitMqConnectionString};
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "tfg-queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            services.AddTransient<IModel>(sp => channel);

            services.AddTransient<IIndexService, IndexElasticsearchService>();
            
            return services;
        }
    }
}