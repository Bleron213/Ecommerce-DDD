using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Ecommerce.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Address).HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }),
                v => JsonSerializer.Deserialize<Address>(v, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,

                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                })!
            )
            .HasColumnType("jsonb");
        }
    }
}
