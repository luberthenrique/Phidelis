using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhidelisMatricula.Application.Interfaces;
using PhidelisMatricula.Application.ViewModels.Aluno;
using PhidelisMatricula.Application.ViewModels.Matricula;
using PhidelisMatricula.Infra.Integracoes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PhidelisMatricula.Services.Api.HostedService
{
    public class IncluirMatriculaHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;


        private int _tempoExecucao;
        Timer _timer;

        public IncluirMatriculaHostedService(
            IServiceProvider serviceProvider,
            ILogger<IncluirMatriculaHostedService> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _tempoExecucao = 600;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteProcess, null, TimeSpan.Zero, TimeSpan.FromSeconds(_tempoExecucao));
            return Task.CompletedTask;
        }

        private async void ExecuteProcess(object state)
        {
            _logger.LogInformation("### Executando serviço de inclusão de matrícula ###");

            using var scope = _serviceProvider.CreateScope();
            var centralDadosClient = scope.ServiceProvider.GetRequiredService<ICentralDadosClient>();
            var alunoAppService = scope.ServiceProvider.GetRequiredService<IAlunoAppService>();
            var matriculaAppService = scope.ServiceProvider.GetRequiredService<IMatriculaAppService>();

            var list = centralDadosClient.ObterNomes(5).Result;

            Random random = new Random();
            foreach (var item in list)
            {
                var idade = random.Next(5, 15);
                var aluno = new AdicionarAlunoViewModel
                {
                    Idade = idade,
                    Nome = item
                };

                var retornoAluno = await alunoAppService.RegisterAsync(aluno);

                var anoLetivo = random.Next(DateTime.Now.Year, DateTime.Now.Year + 5);
                var matricula = new AdicionarMatriculaViewModel
                {
                    IdAluno = retornoAluno.Id,
                    AnoLetivo = anoLetivo,
                    Observacao = "",
                };
                await matriculaAppService.RegisterAsync(matricula);

                _logger.LogInformation($"Matricula gerada para {item}");
            }

            _logger.LogInformation($"Serviço de inclusão de matrícula finalizado {DateTime.Now}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("### Proccess stoping ###");
            _logger.LogInformation($"{DateTime.Now}");
            return Task.CompletedTask;
        }

        public async Task AtualizarTempoExecucao(int segundos)
        {
            await StartAsync(new CancellationToken());
            Thread.Sleep(2000);

            _tempoExecucao = segundos;
            _timer.Dispose();

            await StartAsync(new CancellationToken());
        }

        public int ObterTempoExecucao()
        {
            return _tempoExecucao;
        }

        public async Task Redefinir()
        {
            _tempoExecucao = 600;
            await StartAsync(new CancellationToken());
        }
    }
}