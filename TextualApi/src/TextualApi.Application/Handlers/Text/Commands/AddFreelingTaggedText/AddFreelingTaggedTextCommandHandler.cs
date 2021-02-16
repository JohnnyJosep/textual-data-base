using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TextualApi.Application.Interfaces;

namespace TextualApi.Application.Handlers.Text.Commands.AddFreelingTaggedText
{
    public class AddFreelingTaggedTextCommandHandler : IRequestHandler<AddFreelingTaggedTextCommand, string>
    {
        private readonly IIndexService _indexService;

        public AddFreelingTaggedTextCommandHandler(IIndexService indexService)
        {
            _indexService = indexService;
        }

        public async Task<string> Handle(AddFreelingTaggedTextCommand request, CancellationToken cancellationToken)
        {
            var morfo = request.ToFreeling().ToList();
            var document = await _indexService.FindAsync<Dictionary<string, object>>(request.Index, request.TextId);
            document.Add("morfo", morfo);

            var id = await _indexService.UpdateAsync<Dictionary<string, object>>(document, request.Index,
                request.TextId);

            return id;
        }
    }
}