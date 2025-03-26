using Ecommerce.Domain.Common;
using Ecommerce.Infrastructure.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Repositories.Generics
{
    public interface IGenericRepository<T>
        where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);

        Task Add(T entity);
        void Update(T entity);
    }
}
