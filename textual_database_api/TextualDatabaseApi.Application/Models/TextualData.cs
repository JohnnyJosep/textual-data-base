using AutoMapper;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using TextualDatabaseApi.Application.Mapping;
using TextualDatabaseApi.Domain.Entities;

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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TextEntry, TextualData>()
                .ForMember(
                    d => d.Metadata, 
                    opt => opt.MapFrom(
                        s => JsonSerializer.Deserialize<Dictionary<string, string>>(s.Metadata, null)));
        }
    }
}