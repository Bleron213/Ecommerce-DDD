using Ecommerce.Domain.Common;
using Ecommerce.Domain.Repositories.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> Complete();
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    }
}
