using Ecommerce.Application.Abstractions.Infrastructure;
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


            var bleroni = (_ecommerceDbContext.Customers.Add(new Domain.Entities.Customer
            {
                Id = Guid.Parse("564660d1-7970-4895-91a3-b81bb95a8d03"),
                FirstName = "Bleron",
                LastName = "Qorri",
                Address = "Prishtine",
                PostalCode = "13000",
            })).Entity;
            _ecommerceDbContext.SaveChanges();

            // -------------------------------------------------------------- //

            // ---------- Simulate Products --------------------------------- //

            var tiguan2019 = (_ecommerceDbContext.Products.Add(new Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = "Volkswagen Tiguan 2019",
                Price = 26000
            })).Entity;

            var volkswagenarteon2020 = (_ecommerceDbContext.Products.Add(new Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = "Volkswagen Arteon 2020",
                Price = 28000
            })).Entity;

            var audia42020 = (_ecommerceDbContext.Products.Add(new Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = "Audi A4 2020",
                Price = 20000
            })).Entity;

            var audia62019 = (_ecommerceDbContext.Products.Add(new Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = "Audi A6 2019",
                Price = 28000
            })).Entity;

            var volkswagentiguan2016 = (_ecommerceDbContext.Products.Add(new Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = "Volkswagen Tiguan 2016",
                Price = 25000
            })).Entity;

            _ecommerceDbContext.SaveChanges();

            // -------------------------------------------------------------- //

            // ---------- Simulate Order --------------------------------- //

            _ecommerceDbContext.Orders.Add(new Domain.Entities.Order
            {
                Id = Guid.NewGuid(),
                Customer = bleroni,
                OrderDate = DateTimeOffset.UtcNow,
                OrderItems = new List<Domain.Entities.OrderItem>
                {
                    new Domain.Entities.OrderItem
                    {
                        Id = Guid.NewGuid(),
                        Product = volkswagentiguan2016,
                        Quantity = 2
                    },
                    new Domain.Entities.OrderItem
                    {
                        Id = Guid.NewGuid(),
                        Product = volkswagenarteon2020,
                        Quantity = 5
                    }
                }
            });


            _ecommerceDbContext.Orders.Add(new Domain.Entities.Order
            {
                Id = Guid.NewGuid(),
                Customer = bleroni,
                OrderDate = DateTimeOffset.UtcNow,
                OrderItems = new List<Domain.Entities.OrderItem>
                {
                    new Domain.Entities.OrderItem
                    {
                        Id = Guid.NewGuid(),
                        Product = audia62019,
                        Quantity = 3
                    },
                    new Domain.Entities.OrderItem
                    {
                        Id = Guid.NewGuid(),
                        Product = audia42020,
                        Quantity = 3
                    }
                }
            });

            _ecommerceDbContext.SaveChanges();

            // -------------------------------------------------------------- //

        }
    }
}
