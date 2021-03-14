#nullable enable
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Services.Contexts.SearchServices.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyPrime.Services.Contexts.SearchServices.Queries
{
    public class SearchQuery : IRequest<IQueryable<MemberSearchDto>>
    {
    }

    public class SearchQueryHandler : IRequestHandler<SearchQuery, IQueryable<MemberSearchDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IQueryable<MemberSearchDto>> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.SearchRepository.Query
                .Include(i => i.Accounts)
                .ThenInclude(i => i.Company)
                .Select(s => new MemberSearchDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    Accounts = s.Accounts.Select(a => new AccountSearchDto
                    {
                        AccountId = a.Id,
                        CompanyName = a.Company.Name,
                        Balance = a.Balance,
                        Status = a.AccountStatus.ToString().ToUpper()
                    }).ToList()
                }).AsQueryable();
            return Task.FromResult(result);
        }
    }
}