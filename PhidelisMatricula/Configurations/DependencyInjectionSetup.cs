using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PhidelisMatricula.Application.Interfaces;
using PhidelisMatricula.Application.Services;
using PhidelisMatricula.Domain.Core.Interfaces;
using PhidelisMatricula.Domain.Core.Notifications;
using PhidelisMatricula.Domain.Entities.Repository;
using PhidelisMatricula.Infra.Data.Repository;
using PhidelisMatricula.Infra.Data.UoW;
using PhidelisMatricula.Infra.Integracoes;
using System;

namespace PhidelisMatricula.Presentation.Configurations
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjectionSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Application
            services.AddScoped<IAlunoAppService, AlunoAppService>();
            services.AddScoped<IMatriculaAppService, MatriculaAppService>();
            #endregion

            #region Domain - Core
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            #endregion

            #region Infra - Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IMatriculaRepository, MatriculaRepository>();
            #endregion

            services.AddScoped<ICentralDadosClient, CentralDadosClient>();
        }
    }
}