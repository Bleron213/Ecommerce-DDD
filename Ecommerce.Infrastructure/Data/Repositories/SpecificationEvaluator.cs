using Ecommerce.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.Skip != null)
            {
                query = query.Skip(spec.Skip.Value);
            }

            if(spec.Take != null)
            {
                query = query.Take(spec.Take.Value);
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // Secondary result to support includable strings
            query = spec.IncludeStrings.Aggregate(query,(current, include) => current.Include(include));

            return query;
        }
    }
}
