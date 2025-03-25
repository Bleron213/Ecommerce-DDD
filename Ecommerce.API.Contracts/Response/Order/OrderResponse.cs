using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.API.Contracts.Response.Order
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public required decimal TotalPrice { get; set; }
    }
}
