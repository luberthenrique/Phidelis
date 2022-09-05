using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhidelisMatricula.Infra.Data.Context;
using System;

namespace PhidelisMatricula.Presentation.Configurations
{
    public static class DatabaseSetup
    {
        public static void AddDatabaseSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(defaultConnectionString, ServerVersion.AutoDetect(defaultConnectionString)));

            services.AddMemoryCache();
        }
    }
}