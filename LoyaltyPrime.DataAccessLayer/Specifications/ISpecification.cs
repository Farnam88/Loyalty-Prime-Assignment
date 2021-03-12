using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.DataAccessLayer.Specifications
{
    public interface ISpecification<TEntity> where TEntity : BaseModel
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        void BuildCriteria(Expression<Func<TEntity, bool>> criteria);
        void BuildIncludes(Expression<Func<TEntity, object>> include);
    }

    public interface ISpecification<TEntity, TResult> where TEntity : BaseModel
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Expression<Func<TEntity, TResult>> Selector { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }

        void BuildSelector(Expression<Func<TEntity, TResult>> selector);
        void BuildCriteria(Expression<Func<TEntity, bool>> criteria);
        void BuildIncludes(Expression<Func<TEntity, object>> include);
    }
}