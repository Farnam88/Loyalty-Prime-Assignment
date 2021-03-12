using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.MemberServices.Commands
{
    public class CreateMemberCommand : IRequest<ResultModel<int>>
    {
        public CreateMemberCommand(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class CreateMemberCommandHandler : BaseRequestHandler<CreateMemberCommand, ResultModel<int>>
    {
        public CreateMemberCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResultModel<int>> Handle(CreateMemberCommand request,
            CancellationToken cancellationToken)
        {
            var member = new Member(request.Name, request.Address);
            await Uow.MemberRepository.AddAsync(member, cancellationToken);
            await Uow.CommitAsync(cancellationToken);
            return ResultModel<int>.Success(201, "Member created", member.Id);
        }
    }
}