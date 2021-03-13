#nullable enable
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Services.Contexts.Search1Services.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyPrime.Services.Contexts.Search1Services.Queries
{
    public class SearchQuery : IRequest<IQueryable<MemberSearchDro>>
    {
    }

    public class SearchQueryHandler : IRequestHandler<SearchQuery, IQueryable<MemberSearchDro>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IQueryable<MemberSearchDro>> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.SearchRepository.Query
                .Include(i => i.Accounts)
                .Select(s => new MemberSearchDro(s.Id, s.Name, s.Address, s.Accounts
                    .Select(a => new AccountSearchDto(a.Id, a.Company.Name, a.Balance, a.AccountStatus.ToString())
                    )
                )).AsQueryable();
            return Task.FromResult(result);
        }
    }
}