using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TextualApi.Application.Interfaces;

namespace TextualApi.Application.Handlers.Text.Commands.IndexText
{
    public class IndexTextCommandHandler : IRequestHandler<IndexTextCommand, string>
    {
        private readonly IIndexService _indexService;

        public IndexTextCommandHandler(IIndexService indexService)
        {
            _indexService = indexService;
        }

        public async Task<string> Handle(IndexTextCommand request, CancellationToken cancellationToken)
        {
            var document = request.ToKeyValue();
            var id = await _indexService.IndexAsync(document, request.Index);
            return id;
        }
    }
}