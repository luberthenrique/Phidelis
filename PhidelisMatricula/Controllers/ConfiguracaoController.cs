using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhidelisMatricula.Application.Interfaces;
using PhidelisMatricula.Domain.Core.Notifications;
using PhidelisMatricula.Services.Api.HostedService;
using System.Net;
using System.Threading.Tasks;

namespace PhidelisMatricula.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracaoController : ApiController
    {
        private readonly IncluirMatriculaHostedService _incluirMatriculaHostedService;
        private readonly IMatriculaAppService _matriculaAppService;
        private readonly IAlunoAppService _alunoAppService;
        public ConfiguracaoController(
            IncluirMatriculaHostedService hostedService,
            IAlunoAppService alunoAppService,
            IMatriculaAppService matriculaAppService,
            INotificationHandler<DomainNotification> notifications) : base(notifications)
        {
            _incluirMatriculaHostedService = hostedService as IncluirMatriculaHostedService;
            _alunoAppService = alunoAppService;
            _matriculaAppService = matriculaAppService;
        }

        /// <summary>
        /// Obter tempo de execução de serviço de inclusão de matrículas
        /// </summary>
        /// <returns></returns>
        [HttpGet("maticulas/tempo-atualizacao")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTempoAtualizacao()
        {
            try
            {
                var tempoExecucao = _incluirMatriculaHostedService.ObterTempoExecucao();

                return Ok($"{tempoExecucao} segundos");
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Alterar tempo de execução de serviço de inclusão de matrículas
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// <b>Observações:</b><br /><br />
        /// O tempo padrão de execução do serviço é de <b>600 segundos</b>. <br />
        /// O tempo de execução do serviço é armazenado na <b>memória do servidor</b>, sendo necessário efetuar a <b>alteração</b> do mesmo <b>sempre que o serviço for reniciado.</b>
        /// </remarks>
        [HttpPut("maticulas/tempo-atualizacao")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> PostTempoAtualizacao([FromQuery]int tempoAtualizacao)
        {
            try
            {
                await _incluirMatriculaHostedService.AtualizarTempoExecucao(tempoAtualizacao);

                return Ok();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Redefine padrões da aplicação e apaga todos os dados do banco de dados
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// <b>Observações:</b><br /><br />
        /// O serviço será parado para execução da limpeza e será iniciado novamente no final do processamento.
        /// </remarks>
        [HttpDelete("clear")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> LimparBancoDeDados()
        {
            try
            {
                await _incluirMatriculaHostedService.StopAsync(new System.Threading.CancellationToken());
                
                await _matriculaAppService.Truncate();
                await _alunoAppService.Truncate();

                await _incluirMatriculaHostedService.Redefinir();

                return Ok();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
