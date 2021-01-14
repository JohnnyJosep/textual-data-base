using Microsoft.Extensions.DependencyInjection;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.UnitOfWork;
using TextualDatabaseApi.Infrastructure.Services;
using TextualDatabaseApi.Infrastructure.UnitOfWork;

namespace TextualDatabaseApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTextualDatabaseInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWorkContainer>();
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddScoped<IApplicationEventService, ApplicationEventService>();
            return services;
        }
    }
}
