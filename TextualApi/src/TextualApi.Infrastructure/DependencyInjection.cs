using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using TextualApi.Application.Interfaces;
using TextualApi.Infrastructure.Services;
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

            services.AddTransient<IIndexService, IndexElasticsearchService>();
            
            return services;
        }
    }
}