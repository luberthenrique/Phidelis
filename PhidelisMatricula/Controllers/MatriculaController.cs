using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhidelisMatricula.Application.Interfaces;
using PhidelisMatricula.Application.ViewModels;
using PhidelisMatricula.Application.ViewModels.Matricula;
using PhidelisMatricula.Core.Application.ViewModels;
using PhidelisMatricula.Domain.Core.Notifications;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PhidelisMatricula.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculaController : ApiController
    {
        private readonly IMatriculaAppService _matriculaAppService;
        public MatriculaController(
            IMatriculaAppService matriculaAppService,
            INotificationHandler<DomainNotification> notifications) : base(notifications)
        {
            _matriculaAppService = matriculaAppService;
        }

        /// <summary>
        /// Obtém matrículas
        /// </summary>
        /// <param name="pagina">Número da página</param>
        /// <param name="quantidadeDadosPagina">Quantidade de dados a ser exibido por página.</param>
        /// <param name="nomeAluno">Filtrar dados pelo nome do aluno.</param>
        /// <param name="status">Filtrar dados pelo status da matrícula.</param>     
        /// <param name="anoLetivo">Filtrar dados pelo ano letivo.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Observações:</b><br /><br />
        /// Utilizar os seguintes status:<br />
        /// <li>1 - Matriculado;</li>
        /// <li>2 - Trancada;</li>
        /// <li>3 - Cancelada;</li>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(ConsultaPaginadaViewModel<MatriculaViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMatricula(int pagina = 1, int quantidadeDadosPagina = 50, string nomeAluno = null, int? status = null, int? anoLetivo = null)
        {
            var matriculas = await _matriculaAppService.GetAllAsync(pagina,quantidadeDadosPagina, status, anoLetivo, nomeAluno);

            return Response(matriculas);
        }


        /// <summary>
        /// Obtém matrícula pelo seu ID
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<MatriculaViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetMatricula(long id)
        {
            var matricula = await _matriculaAppService.GetByIdAsync(id);

            if (matricula is null)
                return NoContent();

            return Response(matricula);
        }

        /// <summary>
        /// Altera matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <param name="matricula">Dados da matrícula</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Observações:</b><br /><br />
        /// Utilizar os seguintes status:<br />
        /// <li>1 - Matriculado;</li>
        /// <li>2 - Trancada;</li>
        /// <li>3 - Cancelada;</li><br />
        /// O range de ano letivo permitido é do <b>ano atual</b> até o <b>ano atual mais 5 anos</b>.<br />
        /// É permitido efetuar múltiplos cadastros de matrícula para o mesmo aluno, desde que exista somente uma patricula com o status 1(Matriculado) para um ano letivo.
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> PutMatricula(long id, [FromBody] AlterarMatriculaViewModel matricula)
        {
            if (!await _matriculaAppService.AnyAsync(id))
                return NoContent();

            await _matriculaAppService.UpdateAsync(id, matricula);

            return Response(matricula);
        }

        /// <summary>
        /// Adiciona matrícula
        /// </summary>
        /// <param name="matricula">Dados da matrícula</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Observações:</b><br /><br />
        /// O range de ano letivo permitido é do <b>ano atual</b> até o <b>ano atual mais 5 anos</b>.<br />
        /// Toda matrícula será cadastrada no sistema com o status <b>1(Matriculado)</b>.<br />
        /// É permitido efetuar múltiplos cadastros de matrícula para o mesmo aluno, desde que exista somente uma patricula com o status <b>1(Matriculado)</b> para <b>um ano letivo</b>.<br />
        /// </remarks>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostMatricula([FromBody] AdicionarMatriculaViewModel matricula)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var retorno = await _matriculaAppService.RegisterAsync(matricula);

            return Response(retorno);
        }

        /// <summary>
        /// Deleta matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteMatricula(long id)
        {
            if (!await _matriculaAppService.AnyAsync(id))
                return NoContent();

            await _matriculaAppService.RemoveAsync(id);

            return Response();
        }

    }
}
