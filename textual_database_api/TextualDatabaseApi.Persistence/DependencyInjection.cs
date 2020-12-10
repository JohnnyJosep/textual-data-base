using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TextualDatabaseApi.Application.Interfaces;

namespace TextualDatabaseApi.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTextualDatabasePersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TextualDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<ITextualDbContext>(provider => provider.GetService<TextualDbContext>());
            
            return services;
        }
    }
}