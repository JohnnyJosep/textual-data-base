using System.Threading.Tasks;

namespace TextualApi.Application.Interfaces
{
    public interface IIndexService
    {
        Task<string> IndexAsync<TDocument>(TDocument document, string index) where TDocument : class;
        Task<TDocument> FindAsync<TDocument>(string index, string searchId) where TDocument : class;
        Task<string> UpdateAsync<TDocument>(dynamic document, string index, string searchId) where TDocument : class;
    }
}