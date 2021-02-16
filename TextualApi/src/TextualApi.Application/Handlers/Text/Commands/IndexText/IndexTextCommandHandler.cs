using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TextualApi.Application.Interfaces;

namespace TextualApi.Application.Handlers.Text.Commands.IndexText
{
    public class IndexTextCommandHandler : IRequestHandler<IndexTextCommand, string>
    {
        private readonly IIndexService _indexService;
        private readonly IMessageQueueService _messageQueueService;

        public IndexTextCommandHandler(IIndexService indexService, IMessageQueueService messageQueueService)
        {
            _indexService = indexService;
            _messageQueueService = messageQueueService;
        }

        public async Task<string> Handle(IndexTextCommand request, CancellationToken cancellationToken)
        {
            var document = request.ToKeyValue();
            var id = await _indexService.IndexAsync(document, request.Index);

            await _messageQueueService.EnqueueText(id, request.Text);
            
            return id;
        }
    }
}