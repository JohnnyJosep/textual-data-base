using  AutoMapper;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using TextualDatabaseApi.Application.Behaviors;

namespace TextualDatabaseApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTextualDatabaseApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddMemoryCache();

            return services;
        }
    }
}