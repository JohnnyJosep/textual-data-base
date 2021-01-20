using System.Collections.Generic;
using System.Text.Json;
using MediatR;

namespace TextualApi.Application.Handlers.Text.Commands.IndexText
{
    public class IndexTextCommand : IRequest<string>
    {
        public string Index { get; set; }
        public string Text { get; set; }
        public Dictionary<string, JsonElement> Metadata { get; set; }

        public Dictionary<string, object> ToKeyValue()
        {
            var dic = new Dictionary<string, object> {{"text", Text}};

            foreach (var (key, value) in Metadata)
            {
                switch (value.ValueKind)
                {
                    case JsonValueKind.Number:
                        dic.Add(key, value.GetDecimal());
                        break;
                    case JsonValueKind.String: 
                        dic.Add(key, value.GetString());
                        break;
                }
            }

            return dic;
        }
    }
}