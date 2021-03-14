#nullable enable
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Services.Contexts.SearchServices.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyPrime.Services.Contexts.SearchServices.Queries
{
    public class SearchQuery : IRequest<IQueryable<MemberSearchDro>>
    {
    }

    public class SearchQueryHandler : IRequestHandler<SearchQuery, IQueryable<MemberSearchDro>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IQueryable<MemberSearchDro>> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.SearchRepository.Query
                .Include(i => i.Accounts)
                .ProjectTo<MemberSearchDro>(_mapper.ConfigurationProvider);
            return Task.FromResult(result);
        }
    }
}