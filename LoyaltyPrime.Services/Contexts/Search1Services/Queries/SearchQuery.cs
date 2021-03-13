#nullable enable
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;
using LoyaltyPrime.Shared.Utilities.Common;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.Search1Services.Queries
{
    public class SearchQuery : IRequest<ResultModel<MemberDto>>
    {
        public string MemberName { get; set; }
        public string Address { get; set; }
        public RangeObject<double, double>? BalanceRange { get; set; }
        public string AccountStatus { get; set; }
    }

    public class SearchQueryHandler : IRequestHandler<SearchQuery, ResultModel<MemberDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ResultModel<MemberDto>> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}