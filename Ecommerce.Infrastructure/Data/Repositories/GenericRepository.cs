using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Repositories.Generics;
using Ecommerce.Infrastructure.Data.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IEcommerceDbContext _ecommerceDbContext;

        public GenericRepository(IEcommerceDbContext ecommerceDbContext)
        {
            _ecommerceDbContext = ecommerceDbContext;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _ecommerceDbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _ecommerceDbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_ecommerceDbContext.Set<T>().AsQueryable(), spec);
        }

        public async Task Add(T entity)
        {
            await _ecommerceDbContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _ecommerceDbContext.Set<T>().Attach(entity);
            _ecommerceDbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _ecommerceDbContext.Set<T>().Remove(entity);
        }
    }
}
