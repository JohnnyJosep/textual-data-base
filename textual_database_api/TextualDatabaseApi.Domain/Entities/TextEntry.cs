using System.Collections.Generic;
using TextualDatabaseApi.Domain.Base;

namespace TextualDatabaseApi.Domain.Entities
{
    public class TextEntry : IHasDomainEvent
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string PartOfSpeech { get; set; }
        public string Metadata { get; set; }


        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
