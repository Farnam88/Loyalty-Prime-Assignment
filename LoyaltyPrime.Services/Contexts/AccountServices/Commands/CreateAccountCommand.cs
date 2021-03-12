﻿using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Common.Base;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountServices.Commands
{
    public class CreateAccountCommand : IRequest<ResultModel<int>>
    {
        public CreateAccountCommand()
        {
        }

        public CreateAccountCommand(int companyId, int memberId)
        {
            CompanyId = companyId;
            MemberId = memberId;
        }

        public int CompanyId { get; set; }
        public int MemberId { get; set; }
    }

    public class CreateAccountCommandHandler : BaseRequestHandler<CreateAccountCommand, ResultModel<int>>
    {
        public CreateAccountCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResultModel<int>> Handle(CreateAccountCommand request,
            CancellationToken cancellationToken)
        {
            var company =
                await Uow.CompanyRepository.GetByIdAsync(request.CompanyId, cancellationToken);
            if (company == null)
                return ResultModel<int>.NotFound(nameof(Company));

            var member = await Uow.MemberRepository.GetByIdAsync(request.MemberId, cancellationToken);
            if (member == null)
                return ResultModel<int>.NotFound(nameof(Member));

            var account = new Account(request.MemberId, request.CompanyId, 0, AccountState.Active);

            await Uow.AccountRepository.AddAsync(account, cancellationToken);

            await Uow.CommitAsync(cancellationToken);

            return ResultModel<int>.Success(201, $"Account has been created for {member.Name}", account.Id);
        }
    }
}