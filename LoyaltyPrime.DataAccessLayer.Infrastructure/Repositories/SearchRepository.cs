using System.Linq;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataLayer;
using LoyaltyPrime.Models;
using LoyaltyPrime.Shared.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyPrime.DataAccessLayer.Infrastructure.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        public IQueryable<Member> Query => _query;


        private readonly LoyaltyPrimeContext _context;
        private IQueryable<Member> _query;

        public SearchRepository(LoyaltyPrimeContext context)
        {
            Preconditions.CheckNull(context, nameof(LoyaltyPrimeContext));
            _context = context;
            var entities = context.Set<Member>();
            Preconditions.CheckNull(entities);
            _query = entities.AsQueryable();
        }
    }
}