using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using MediatR;

namespace LoyaltyPrime.Services.Common.Base
{
    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected readonly IUnitOfWork Uow;

        protected BaseRequestHandler(IUnitOfWork uow)
        {
            Uow = uow;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}