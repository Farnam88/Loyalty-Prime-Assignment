using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Common.Specifications.CompanySpec;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountServices.Commands
{
    public class CreateAccountCommand : IRequest<ResultModel>
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

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ResultModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            //todo: Finding existing Company should move to a MediatR query
            var companySpecification = new FindCompanySpecification(request.CompanyId);
            var company =
                await _unitOfWork.CompanyRepository.FirstOrDefaultAsync(companySpecification, cancellationToken);
            if (company == null)
                return ResultModel.Fail(404, "The required Company does not exist!", ErrorTypes.NotFound);
            
            //todo: Finding existing member should move to a MediatR query
            var memberSpecification = new FindMemberSpecification(request.MemberId);
            var member = await _unitOfWork.MemberRepository.FirstOrDefaultAsync(memberSpecification, cancellationToken);
            if (member == null)
                return ResultModel.Fail(404, "The required member does not exist!", ErrorTypes.NotFound);
            
            var account = new Account(request.MemberId, request.CompanyId, 0, AccountState.Active);
            await _unitOfWork.AccountRepository.AddAsync(account, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return ResultModel.Success(201, $"Account has been created for {member.Name}");
        }
    }
}