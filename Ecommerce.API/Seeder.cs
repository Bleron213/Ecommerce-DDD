using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API
{
    public class Seeder
    {
        private readonly IEcommerceDbContext _ecommerceDbContext;
        private readonly ICurrentUserService _currentUserService;

        public Seeder(IEcommerceDbContext ecommerceDbContext, ICurrentUserService currentUserService)
        {
            _ecommerceDbContext = ecommerceDbContext;
            _currentUserService = currentUserService;
        }

        public async void Seed()
        {
            _ecommerceDbContext.Database.EnsureDeleted();
            _ecommerceDbContext.Database.EnsureCreated();

            //_ecommerceDbContext.OrderItems.ExecuteDelete();
            //_ecommerceDbContext.Orders.ExecuteDelete();
            //_ecommerceDbContext.Products.ExecuteDelete();
            //_ecommerceDbContext.Customers.ExecuteDelete();


            // ---------- Simulate Customer --------------------------------- //

            var bleroni = (_ecommerceDbContext.Customers.Add(new Customer(Guid.Parse("564660d1-7970-4895-91a3-b81bb95a8d03"), "Bleron", "Qorri", new Address("Prishtine", "13000")))).Entity;
            _ecommerceDbContext.SaveChanges();

            //// -------------------------------------------------------------- //

            //// ---------- Simulate Products --------------------------------- //

            var tiguan2019 = (_ecommerceDbContext.Products.Add(new Product("Volkswagen Tiguan 2019", new Price(26000)))).Entity;
            var volkswagenarteon2020 = (_ecommerceDbContext.Products.Add(new Product("Volkswagen Arteon 2020", new Price(28000)))).Entity;
            var audia42020 = (_ecommerceDbContext.Products.Add(new Product("Audi A4 2020", new Price(28000)))).Entity;
            var audia62019 = (_ecommerceDbContext.Products.Add(new Product("Audi A6 2019", new Price(28000)))).Entity;
            var volkswagentiguan2016 = (_ecommerceDbContext.Products.Add(new Product("Volkswagen Tiguan 2016", new Price(25000)))).Entity;

            _ecommerceDbContext.SaveChanges();

            //// -------------------------------------------------------------- //

            //// ---------- Simulate Order --------------------------------- //

            var order1 = new Order(DateTimeOffset.UtcNow, bleroni.Id);
            order1.AddOrderItem(volkswagentiguan2016, 2);
            order1.AddOrderItem(volkswagenarteon2020, 5);

            var order2 = new Order(DateTimeOffset.UtcNow, bleroni.Id);
            order2.AddOrderItem(audia62019, 3);
            order2.AddOrderItem(audia42020, 3);

            _ecommerceDbContext.Orders.Add(order1);
            _ecommerceDbContext.Orders.Add(order2);
            _ecommerceDbContext.SaveChanges();

            // -------------------------------------------------------------- //

        }
    }
}
