using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).HasMaxLength(1000);

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
