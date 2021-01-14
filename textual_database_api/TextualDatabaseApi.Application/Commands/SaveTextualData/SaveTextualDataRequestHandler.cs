using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using TextualDatabaseApi.Application.Extensions;
using TextualDatabaseApi.Application.Models;
using TextualDatabaseApi.Application.Models.ApplicationEvents;
using TextualDatabaseApi.Application.UnitOfWork;
using TextualDatabaseApi.Domain.Entities;

namespace TextualDatabaseApi.Application.Commands.SaveTextualData
{
    public class SaveTextualDataRequestHandler : IRequestHandler<SaveTextualDataRequest, TextualData>
    {
        private readonly ILogger<SaveTextualDataRequestHandler> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SaveTextualDataRequestHandler(IUnitOfWork uow, IMapper mapper, ILogger<SaveTextualDataRequestHandler> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TextualData> Handle(SaveTextualDataRequest request, CancellationToken cancellationToken)
        {
            IDbContextTransaction transaction = null;
            try
            {
                transaction = await _uow.BeginTransactionAsync(cancellationToken);

                var text = new TextEntry {Text = request.Text, Metadata = request.Metadata.AsJson()};
                await _uow.TextEntryRepository.AddAsync(text, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                foreach (var key in request.Metadata.Keys)
                {
                    var attr = await _uow.TextAttributeRepository.GetOrAddAsync(
                        new TextAttribute {Name = key}, 
                        a => a.Name == key, 
                        cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);

                    var value = new AttributeValue
                        {TextEntryId = text.Id, TextAttributeId = attr.Id, Value = request.Metadata[key]};
                    await _uow.AttributeValueRepository.AddAsync(value, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);
                }

                var data = _mapper.Map<TextualData>(text);
                
                await _uow.CommitTransactionAsync(cancellationToken, new TextualDataCreateEvent(data));
                
                _logger.LogInformation("Commit transaction");
                
                return data;
            }
            catch
            {
                if (transaction != null)
                {
                    await _uow.RollbackTransactionAsync(cancellationToken);
                }
                throw;
            }
        }
    }
}