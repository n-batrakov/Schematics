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
                .WithEntity(entity => entity
                    .Source<IDataSource>()
                    .Version("1")
                    .Name("Entity")
                    .WithProperty("Id", new NumberType())
                    .WithProperty("Name", new StringType())
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
