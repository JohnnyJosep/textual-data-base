using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TextualDatabaseApi.Application.Models;
using TextualDatabaseApi.Application.UnitOfWork;

namespace TextualDatabaseApi.Application.Queries.GetTextualData
{
    public class GetTextualDataRequestHandler : IRequestHandler<GetTextualDataRequest, IEnumerable<TextualData>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTextualDataRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TextualData>> Handle(GetTextualDataRequest request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.TextEntryRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<TextualData>>(data);
            return result;
        }
    }
}