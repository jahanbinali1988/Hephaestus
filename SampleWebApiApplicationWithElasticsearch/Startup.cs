using Hephaestus.Repository.Elasticsearch.Configure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Hephaestus.Repository.Elasticsearch.Extensions;
using SampleWebApiApplicationWithElasticsearch.Persistence.MappingConfiguration;
using SampleWebApiApplicationWithElasticsearch.Extensions;
using SampleWebApiApplicationWithElasticsearch.Persistence.EventProcessing;
using Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent;
using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithElasticsearch.Persistence;

namespace SampleWebApiApplicationWithElasticsearch
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDependencies(Configuration);
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SampleWebApiApplicationWithElasticsearch", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleWebApiApplicationWithElasticsearch v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
