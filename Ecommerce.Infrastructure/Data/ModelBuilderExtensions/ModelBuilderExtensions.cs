using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql.NameTranslation;

namespace Ecommerce.Infrastructure.Data.ModelBuilderExtensions
{
    public static class ModelBuilderExtensions
    {
        public static void ChangeEntitiesToSnakeCase(this ModelBuilder builder)
        {
            var mapper = new NpgsqlSnakeCaseNameTranslator();
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    var storeObjectIdentifier = StoreObjectIdentifier.Create((IMutableEntityType)property.DeclaringType, StoreObjectType.Table);
                    if (storeObjectIdentifier.HasValue)
                    {
                        property.SetColumnName(mapper.TranslateMemberName(property.GetColumnName(storeObjectIdentifier.Value)!));
                    }
                }

                entity.SetTableName(mapper.TranslateTypeName(entity.GetTableName()!));
            }
        }
    }
}
