using System.Linq;
using LoyaltyPrime.Models;

namespace LoyaltyPrime.DataAccessLayer.Repositories
{
    public interface ISearchRepository
    {
        IQueryable<Member> Query { get; }
    }
}