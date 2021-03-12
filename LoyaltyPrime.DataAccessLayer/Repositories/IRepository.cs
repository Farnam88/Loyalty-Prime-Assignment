using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.DataAccessLayer.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseModel
    {
        /// <summary>
        /// Get first of default of the Dto object
        /// </summary>
        /// <param name="spec">an implementation of ISpecificationWithSelector</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="TResult">Expected Dto result</typeparam>
        /// <returns>Task of Dto object</returns>
        Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> spec,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get first of default of Entity
        /// </summary>
        /// <param name="spec">an implementation of ISpecification Entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task of Entity</returns>
        Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get IList of the Dto object
        /// </summary>
        /// <param name="spec">an implementation of ISpecificationWithSelector</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="TResult">Expected Dto result</typeparam>
        /// <returns>Task of Dto object set</returns>
        Task<IList<TResult>> GetAllAsync<TResult>(ISpecification<TEntity, TResult> spec,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get IList of the Entity object
        /// </summary>
        /// <param name="spec">an implementation of ISpecification</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task of Entity set</returns>
        Task<IList<TEntity>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Count the records
        /// </summary>
        /// <param name="criteria">predication</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>number of records in Int32</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria,
            CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Get Entity by Id
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add Entity
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add IList of Entities
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        Task AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete by Id
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete by Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);
    }
}