#nullable enable
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.DataAccessLayer.Specifications
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseModel
    {
        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteria = null!)
        {
            Criteria = criteria;
        }

        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; } =
            new List<Expression<Func<TEntity, object>>>();

        public void BuildCriteria(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void BuildIncludes(Expression<Func<TEntity, object>> include)
        {
            Includes.Add(include);
        }
    }

    public abstract class BaseSpecification<TEntity, TResult> : ISpecification<TEntity, TResult>
        where TEntity : BaseModel where TResult : class
    {
        public List<Expression<Func<TEntity, object>>> Includes { get; } =
            new List<Expression<Func<TEntity, object>>>();

        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
        public Expression<Func<TEntity, TResult>> Selector { get; private set; }

        protected BaseSpecification(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? criteria = null!
        )
        {
            Criteria = criteria;
            Selector = selector;
        }

        public void BuildCriteria(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void BuildIncludes(Expression<Func<TEntity, object>> include)
        {
            Includes.Add(include);
        }

        public void BuildSelector(Expression<Func<TEntity, TResult>> selector)
        {
            Selector = selector;
        }
    }
}