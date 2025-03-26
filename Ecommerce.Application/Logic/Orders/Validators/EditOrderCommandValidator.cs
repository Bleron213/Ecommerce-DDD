using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Application.Logic.Orders.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Logic.Orders.Validators
{
    public class EditOrderCommandValidator : AbstractValidator<EditOrderCommand>
    {
        public EditOrderCommandValidator(IEcommerceDbContext ecommerceDbContext)
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage("Malformed request body");

            RuleFor(x => x.Request.OrderItems)
                .NotNull()
                .NotEmpty()
                .WithMessage("Order must have at least one item");

            RuleForEach(x => x.Request.OrderItems)
                .Must(x => x.Quantity > 0)
                .WithMessage("Quantity must be greater than zero");

            RuleFor(x => x.Request.OrderItems)
                .MustAsync(async (orderItems, cancellationToken) =>
                {
                    var productIds = orderItems.Select(item => item.ProductId).ToList();

                    var productIdsExist = await ecommerceDbContext.Products
                        .Where(p => productIds.Contains(p.Id))
                        .Select(p => p.Id)
                        .ToListAsync(cancellationToken);

                    return productIds.All(id => productIdsExist.Contains(id));
                })
                .WithMessage("At least one or more product ids were not found");
        }
    }
}
