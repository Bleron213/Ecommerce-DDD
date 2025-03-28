﻿using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Application.Services;

namespace Ecommerce.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAPIServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }
    }
}
