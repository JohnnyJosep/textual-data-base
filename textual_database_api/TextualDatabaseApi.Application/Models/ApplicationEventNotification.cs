using MediatR;

namespace TextualDatabaseApi.Application.Models
{
    public class ApplicationEventNotification<TApplicationEvent> : INotification where TApplicationEvent : ApplicationEvent
    {
        public TApplicationEvent ApplicationEvent { get; }
        
        public ApplicationEventNotification(TApplicationEvent applicationEvent)
        {
            ApplicationEvent = applicationEvent;
        }
    }
}