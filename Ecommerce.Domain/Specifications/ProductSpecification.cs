using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data.Specifications;

namespace Ecommerce.Domain.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(List<Guid> productIds) : base(x => productIds.Contains(x.Id))
        {

        }

    }
}
