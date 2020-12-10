using System.Collections.Generic;
using MediatR;
using TextualDatabaseApi.Application.Models;

namespace TextualDatabaseApi.Application.Commands.SaveTextualData
{
    public class SaveTextualDataRequest : IRequest<TextualData>
    {
        public string Text { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}