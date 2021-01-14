using System;

namespace TextualDatabaseApi.Application.Models
{
    public class ApplicationEvent
    {
        public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
        
        protected ApplicationEvent()
        {
            DateOccurred = DateTimeOffset.UtcNow;
        }
    }
}