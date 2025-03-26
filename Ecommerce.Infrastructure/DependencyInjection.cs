using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Data.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data.Repositories;
using Ecommerce.Domain.Repositories.Generics;

namespace Ecommerce.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnection");
            //NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

            services.AddScoped<AuditableEntityInterceptor>();
            services.AddScoped<DispatchDomainEventsInterceptor>();
            services.AddScoped<AuditTrailInterceptor>();

            services.AddDbContext<IEcommerceDbContext, EcommerceDbContext>((sp, options) =>
            {
                options.AddInterceptors
                (
                    sp.GetService<DispatchDomainEventsInterceptor>()!,
                    sp.GetService<AuditableEntityInterceptor>()!,
                    sp.GetService<AuditTrailInterceptor>()!
                );
                options.UseNpgsql(connectionString);
            });

            return services;
        }


        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));  // Register Generic Repository
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
