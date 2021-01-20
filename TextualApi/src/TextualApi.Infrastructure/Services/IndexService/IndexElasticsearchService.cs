using System.Threading.Tasks;
using Nest;
using TextualApi.Application.Interfaces;

namespace TextualApi.Infrastructure.Services.IndexService
{
    public class IndexElasticsearchService : IIndexService
    {
        private readonly ElasticClient _client;

        public IndexElasticsearchService(ElasticClient client)
        {
            _client = client;
        }

        public async Task<string> IndexAsync(object document, string index)
        {
            var resp = await _client.IndexAsync(document, i => i.Index(index));
            if (!resp.IsValid)
            {
                throw new NotIndexedException();
            }

            return resp.Id;
        }
    }
}