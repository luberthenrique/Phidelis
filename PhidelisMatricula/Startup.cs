using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhidelisMatricula.Infra.Integracoes.Models;
using PhidelisMatricula.Presentation.Configurations;
using PhidelisMatricula.Services.Api.HostedService;

namespace PhidelisMatricula
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
            // Setting DBContexts
            services.AddDatabaseSetup(Configuration);

            // WebAPI Config
            services.AddControllers();

            // Swagger Config
            services.AddSwaggerSetup();

            // .NET Native DI Abstraction            
            services.AddDependencyInjectionSetup();

            services.AddControllers();

            services.AddSingleton<IncluirMatriculaHostedService>();
            services.AddSingleton<IHostedService>(c => c.GetRequiredService<IncluirMatriculaHostedService>());

            services.Configure<CentralDadosSettings>(Configuration.GetSection("CentralDadosSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerSetup();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
