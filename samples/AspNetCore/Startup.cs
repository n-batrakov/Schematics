using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Schematics.Api.Rest;
using Schematics.Core;

namespace Schematics.AspNetCore.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSchematicsApi(schema => schema
                .Entity(entity => entity
                    .Name("Entity")
                    .Id("Id", new NumberType())
                    .AddProperty("Name", new StringType())
                )
                
            );
            
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSchematicsApi();
            
            app.UseMvc();
        }
    }
}
