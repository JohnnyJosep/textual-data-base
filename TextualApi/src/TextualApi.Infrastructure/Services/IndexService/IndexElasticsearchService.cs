using System.Linq;
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

        public async Task<string> IndexAsync<TDocument>(TDocument document, string index) where TDocument : class
        {
            var resp = await _client.IndexAsync(document, i => i.Index(index));
            if (!resp.IsValid)
            {
                throw new ElasticsearchInvalidResponseException();
            }

            return resp.Id;
        }

        public async Task<TDocument> FindAsync<TDocument>(string index, string searchId) where TDocument : class
        {
            var resp = await _client.SearchAsync<TDocument>(d => d
                .Index(index)
                .Query(q => q.Term(t => t
                    .Field("_id")
                    .Value(searchId))));
            if (!resp.IsValid)
            {
                throw new ElasticsearchInvalidResponseException();
            }

            var document = resp.Hits.FirstOrDefault()?.Source;
            return document;
        }

        public async Task<string> UpdateAsync<TDocument>(dynamic document, string index, string searchId) where TDocument : class
        {
            var resp = await _client.UpdateAsync<TDocument, dynamic>(searchId, d => d
                .Index(index)
                .Doc(document));
            if (!resp.IsValid)
            {
                throw new ElasticsearchInvalidResponseException();
            }
            
            return resp.Id;
        }
    }
}