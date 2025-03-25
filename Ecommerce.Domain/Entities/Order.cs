using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Order : BaseEntity
    {

        public decimal TotalPrice { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        public Guid CustomerId { get; set; }

        #region Navigations
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        #endregion

    }
}
