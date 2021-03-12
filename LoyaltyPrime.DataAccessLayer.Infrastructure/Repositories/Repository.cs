using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Shared.Utilities.Extensions;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.DataLayer;
using LoyaltyPrime.Models.Bases.CommonEntities;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyPrime.DataAccessLayer.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
    {
        private readonly LoyaltyPrimeContext _context;
        private readonly DbSet<TEntity> _entities;

        public Repository(LoyaltyPrimeContext context)
        {
            Preconditions.CheckNull(context, nameof(LoyaltyPrimeContext));
            _context = context;
            var entities = context.Set<TEntity>();
            Preconditions.CheckNull(entities);
            _entities = entities;
        }

        public async Task<IList<TResult>> GetAllAsync<TResult>(ISpecification<TEntity, TResult> spec,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TResult> query = ApplySpecifications(spec);
            IList<TResult> resultSet = await query.ToListAsync(cancellationToken);
            return resultSet;
        }

        public async Task<IList<TEntity>> GetAllAsync(ISpecification<TEntity> spec,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = ApplySpecifications(spec);
            IList<TEntity> resultSet = await query.ToListAsync(cancellationToken);
            return resultSet;
        }

        public async Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> spec,
            CancellationToken cancellationToken = new CancellationToken())
        {
            IQueryable<TResult> query = ApplySpecifications(spec);
            TResult result = await query.FirstOrDefaultAsync(cancellationToken);
            return result;
        }

        public async Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec,
            CancellationToken cancellationToken = new CancellationToken())
        {
            IQueryable<TEntity> query = ApplySpecifications(spec);
            TEntity result = await query.FirstOrDefaultAsync(cancellationToken);
            return result;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria,
            CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await _entities.Where(criteria).CountAsync(cancellationToken);
            return result;
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            TEntity entity = await _entities
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
            return entity;
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _entities.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Preconditions.CheckNull(entities);
            if (entities.Any())
                await _entities.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            TEntity entity = await _entities.FindAsync(id, cancellationToken);
            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            Preconditions.CheckNull(entity);
            _entities.Remove(entity);
        }

        private IQueryable<TEntity> ApplySpecifications(ISpecification<TEntity> spec)
        {
            IQueryable<TEntity> query = _entities.AsQueryable();

            if (spec.Includes is not null && spec.Includes.Count > 0)
            {
                query = spec.Includes
                    .Aggregate(_entities.AsQueryable(),
                        (current, include) => current.Include(include));
            }

            if (spec.Criteria is not null)
            {
                query = _entities
                    .Where(spec.Criteria);
            }

            return query;
        }

        private IQueryable<TResult> ApplySpecifications<TResult>(ISpecification<TEntity, TResult> spec)
        {
            IQueryable<TEntity> query = _entities.AsQueryable();
            if (spec.Includes is not null && spec.Includes.Count > 0)
                query = spec.Includes
                    .Aggregate(query,
                        (current, include) => current.Include(include));

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);
            IQueryable<TResult> resultQuery = query
                .Select(spec.Selector);
            return resultQuery;
        }
    }
}