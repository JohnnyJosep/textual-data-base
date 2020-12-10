using System.Collections.Generic;
using MediatR;
using TextualDatabaseApi.Application.Models;

namespace TextualDatabaseApi.Application.Queries.GetTextualData
{
    public class GetTextualDataRequest : IRequest<IEnumerable<TextualData>>
    {
        
    }
}