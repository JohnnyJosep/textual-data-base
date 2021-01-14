using System.Collections.Generic;
using MediatR;
using TextualDatabaseApi.Application.Models;

namespace TextualDatabaseApi.Application.Queries.GetTextualData
{
    public class GetTextualDataRequest : IRequest<IEnumerable<TextualData>>
    {
        public string CacheKey => "get_textual_data";
    }
}