using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Schematics.Core;

namespace Schematics.Api.Rest
{
    public static class Extensions
    {
        public static IServiceCollection AddSchematicsApi(this IServiceCollection services,
            Func<EntityProviderBuilder, EntityProviderBuilder> configure)
        {
            services.AddSingleton(x => configure(new EntityProviderBuilder(x)).Build());

            // TODO: Add DataSources, Resolvable services, etc.
            
            return services;
        }

        public static IApplicationBuilder UseSchematicsApi(this IApplicationBuilder app)
        {
            // TODO: Configure routing, serialization,
            
            return app;
        }
    }
}
