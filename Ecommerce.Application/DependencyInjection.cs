using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Ecommerce.Application
{
    public static class DependencyInjection
    {
        //public static IServiceCollection AddDistributedCache(this IServiceCollection services)
        //{
        //    services.AddDistributedMemoryCache();
        //    services.AddScoped<IDistributedCachingService, DistributedCachingService>();
        //    return services;
        //}

        //public static IServiceCollection AddCachingFactory(this IServiceCollection services)
        //{
        //    services.AddDistributedMemoryCache();
        //    services.AddMemoryCache();
        //    services.TryAddScoped<DistributedCachingService>();
        //    services.TryAddScoped<MemoryCacheService>();

        //    services.TryAddScoped<CachingServiceResolver>(serviceProvider =>
        //    {
        //        return key =>
        //        {
        //            switch (key)
        //            {
        //                case CacheType.Memory:
        //                    {
        //                        return serviceProvider.GetService<MemoryCacheService>()!;
        //                    }
        //                case CacheType.Redis:
        //                    {
        //                        return serviceProvider.GetService<DistributedCachingService>()!;
        //                    }
        //                default:
        //                    throw new NotImplementedException($"No known provider for this key {key}");
        //            }
        //        };
        //    });

        //    return services;

        //}

        //public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        //{
        //    services.AddScoped<ISharedAccessSignatureService, SharedAccessSignatureService>();
        //    return services;
        //}

        //public static IServiceCollection RegisterClientMediatR(this IServiceCollection services)
        //{
        //    //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //    services.AddMediatR(cfg =>
        //    {
        //        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        //        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        //        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        //    });

        //    return services;
        //}
    }

}
