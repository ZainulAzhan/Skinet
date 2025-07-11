﻿using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
    {
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        
        if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        if (specification.IsDistinct)
        {
            query = query.Distinct();
        }

        if (specification.IsPagingEnabled)
        {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }

        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query,
        ISpecification<T, TResult> specification)
    {
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        
        if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        var selectQuery = query as IQueryable<TResult>;

        if (specification.Select != null)
        {
            selectQuery = query.Select(specification.Select);
        }

        if (specification.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }

        if (specification.IsPagingEnabled)
        {
            selectQuery = selectQuery?.Skip(specification.Skip).Take(specification.Take);
        }

        return selectQuery ?? query.Cast<TResult>();
    }
}