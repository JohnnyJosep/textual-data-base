using System.Threading.Tasks;

namespace TextualApi.Application.Interfaces
{
    public interface IIndexService
    {
        Task<string> IndexAsync(object document, string index);
    }
}