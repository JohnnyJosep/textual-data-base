namespace TextualDatabaseApi.Domain
{
    public class AttributeValue
    {
        public int TextEntryId { get; set; }
        public int TextAttributeId { get; set; }
        public string Value { get; set; }
        
        public virtual TextEntry TextEntry { get; set; }
        public virtual TextAttribute TextAttribute { get; set; }
    }
}