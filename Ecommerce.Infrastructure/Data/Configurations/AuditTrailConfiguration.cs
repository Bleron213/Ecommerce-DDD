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
    public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
    {
        public void Configure(EntityTypeBuilder<AuditTrail> builder)
        {
            builder.HasKey(x => x.AuditId);

            builder.Property(x => x.AuditId).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedBy).HasMaxLength(100);
            builder.Property(x => x.AffectedEntity).HasMaxLength(50);
            builder.Property(x => x.OldValues).HasColumnType("jsonb").HasConversion(
                        v => JsonSerializer.Serialize(v, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                        }),
                        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,

                            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                        })!
                    );
            builder.Property(x => x.NewValues)
                .HasColumnType("jsonb").HasConversion(
                        v => JsonSerializer.Serialize(v, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                        }),
                        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,

                            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                        })!
                    );
        }
    }

}
  