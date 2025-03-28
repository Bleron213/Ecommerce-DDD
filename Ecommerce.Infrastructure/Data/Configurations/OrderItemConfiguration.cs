﻿using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Ecommerce.Domain.Entities.Shared.ValueObjects;

namespace Ecommerce.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId);

            builder.Property(x => x.Price).HasConversion(
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
