using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TextualDatabaseApi.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTextualDatabasePersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TextualDbContext>(options => options.UseNpgsql(connectionString));
            
            return services;
        }
    }
}