using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Models;

namespace TextualDatabaseApi.Infrastructure.Services
{
    public class ApplicationEventService : IApplicationEventService
    {
        private readonly ILogger<DomainEventService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ApplicationEventService(ILogger<DomainEventService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        
        public Task Publish(ApplicationEvent applicationEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Publishing application event. Event - {event}", applicationEvent.GetType().Name);

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var notification = GetNotificationCorrespondingToApplicationEvent(serviceProvider, applicationEvent);
                var publisher = serviceProvider.GetService<IPublisher>();
                publisher?.Publish(notification, cancellationToken);
            }

            return Task.CompletedTask;
        }
        
        private INotification GetNotificationCorrespondingToApplicationEvent(IServiceProvider serviceProvider, ApplicationEvent applicationEvent)
        {
            return (INotification)ActivatorUtilities.CreateInstance(serviceProvider,
                typeof(ApplicationEventNotification<>).MakeGenericType(applicationEvent.GetType()), applicationEvent);
        }
    }
}