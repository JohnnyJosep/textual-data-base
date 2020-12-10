using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TextualDatabaseApi.Application.Models;
using TextualDatabaseApi.Application.UnitOfWork;
using TextualDatabaseApi.Domain;

namespace TextualDatabaseApi.Application.Commands.SaveTextualData
{
    public class SaveTextualDataRequestHandler : IRequestHandler<SaveTextualDataRequest, TextualData>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SaveTextualDataRequestHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<TextualData> Handle(SaveTextualDataRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _uow.BeginTransactionAsync();

                var text = new TextEntry {Text = request.Text, Metadata = request.Metadata};
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
                
                _uow.CommitTransaction();
                var data = _mapper.Map<TextualData>(text);
                return data;
            }
            catch
            {
                _uow.RollbackTransaction();
                throw;
            }
        }
    }
}