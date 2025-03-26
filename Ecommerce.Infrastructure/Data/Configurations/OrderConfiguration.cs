using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities.ValueObjects;
using System.Text.Json;

namespace Ecommerce.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId);

            builder.Property(x => x.TotalPrice).HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }),
                v => JsonSerializer.Deserialize<Price>(v, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,

                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                })!
            )
            .HasColumnType("jsonb");
        }
    }
}
