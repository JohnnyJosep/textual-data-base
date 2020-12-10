using System.Collections.Generic;
using System.Text.Json.Serialization;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Domain;

namespace TextualDatabaseApi.Application.Models
{
    public class TextualData : IMapFrom<TextEntry>
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("text")]
        public string Text { get; set; }
        
        [JsonPropertyName("pos")]
        public string PartOfSpeech { get; set; }
        
        [JsonPropertyName("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }
}