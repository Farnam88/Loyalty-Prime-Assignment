using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.MemberServices.Commands
{
    public class CreateMemberCommand : IRequest<ResultModel>
    {
        public CreateMemberCommand(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, ResultModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMemberCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = new Member(request.Name, request.Address);
            await _unitOfWork.MemberRepository.AddAsync(member, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return ResultModel.Success(201, "Member created");
        }
    }
}