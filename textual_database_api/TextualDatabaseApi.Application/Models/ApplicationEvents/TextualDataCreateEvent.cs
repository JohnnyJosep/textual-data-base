namespace TextualDatabaseApi.Application.Models.ApplicationEvents
{
    public class TextualDataCreateEvent : ApplicationEvent
    {
        public TextualData TextualData { get; }

        public TextualDataCreateEvent(TextualData textualData)
        {
            TextualData = textualData;
        }
    }
}