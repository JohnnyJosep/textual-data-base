using Microsoft.Extensions.DependencyInjection;
using TextualDatabaseApi.Application.UnitOfWork;
using TextualDatabaseApi.Infrastructure.UnitOfWork;

namespace TextualDatabaseApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTextualDatabaseInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWorkContainer>();
            return services;
        }
    }
}
