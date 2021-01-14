namespace TextualDatabaseApi.Domain
{
    public class TextEntry
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string PartOfSpeech { get; set; }
        public string Metadata { get; set; }
    }
}
