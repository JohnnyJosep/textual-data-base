using System.Collections.Generic;

namespace TextualDatabaseApi.Domain.Base
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}