using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using TextualDatabaseApi.Application.Models;
using TextualDatabaseApi.Application.UnitOfWork;

namespace TextualDatabaseApi.Application.Queries.GetTextualData
{
    public class GetTextualDataRequestHandler : IRequestHandler<GetTextualDataRequest, IEnumerable<TextualData>>
    {
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTextualDataRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<TextualData>> Handle(GetTextualDataRequest request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(request.CacheKey, out TextualData[] result)) return result;
            
            var data = await _unitOfWork.TextEntryRepository.GetAllAsync();
            result = _mapper.Map<TextualData[]>(data);
            
            _cache.Set(request.CacheKey, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(6)));

            return result;
        }
    }
}