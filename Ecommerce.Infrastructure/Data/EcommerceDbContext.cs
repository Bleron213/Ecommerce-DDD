using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data.ModelBuilderExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : DbContext(options), IEcommerceDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
            builder.ChangeEntitiesToSnakeCase();

        }
    }
}
