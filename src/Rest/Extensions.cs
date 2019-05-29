using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Schematics.Core;

namespace Schematics.Api.Rest
{
    public static class Extensions
    {
        public static IServiceCollection AddSchematicsApi(this IServiceCollection services, Func<SchemaBuilder, SchemaBuilder> configure)
        {
            services.AddSingleton(x =>
            {
                var comparer = x.GetService<IEntityComparerProvider>() ?? InvariantIgnoreCaseComparerProvider.Instance;

                return configure(new SchemaBuilder(x, comparer)).Build();
            });

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
