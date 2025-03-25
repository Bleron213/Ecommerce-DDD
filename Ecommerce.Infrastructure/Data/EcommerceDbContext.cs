using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : DbContext(options), IEcommerceDbContext
    {
        public DbSet<Customer> Customer { get; set; }
    }
}
