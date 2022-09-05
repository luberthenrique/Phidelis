using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhidelisMatricula.Application.Interfaces;
using PhidelisMatricula.Application.ViewModels;
using PhidelisMatricula.Application.ViewModels.Aluno;
using PhidelisMatricula.Core.Application.ViewModels;
using PhidelisMatricula.Domain.Core.Notifications;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PhidelisMatricula.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ApiController
    {
        private readonly IAlunoAppService _alunoAppService;
        public AlunoController(
            IAlunoAppService alunoAppService,
            INotificationHandler<DomainNotification> notifications) : base(notifications)
        {
            _alunoAppService = alunoAppService;
        }

        /// <summary>
        /// Obtém alunos
        /// </summary>
        /// <param name="pagina">Número da página</param>
        /// <param name="quantidadeDadosPagina">Quantidade de dados a ser exibido por página</param>
        /// <param name="nome">Filtrar dados pelo nome do aluno</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ConsultaPaginadaViewModel<AlunoViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAluno(int pagina = 1, int quantidadeDadosPagina = 50, string nome = null)
        {
            var alunos = await _alunoAppService.GetAllAsync(pagina, quantidadeDadosPagina, nome);

            return Response(alunos);
        }

        /// <summary>
        /// Obtém aluno pelo seu ID
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAluno(long id)
        {
            var aluno = await _alunoAppService.GetByIdAsync(id);

            if (aluno is null)
                return NoContent();

            return Response(aluno);
        }

        /// <summary>
        /// Altera aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <param name="aluno">Dados do aluno</param>
        /// <returns></returns>
        /// <remarks>
        /// Observações:<br /><br />
        /// A idade permitida na edição de alunos é de <b>5 a 15 anos</b>.
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> PutAluno(long id, [FromBody] AlterarAlunoViewModel aluno)
        {
            if (!await _alunoAppService.AnyAsync(id))
                return NoContent();

            await _alunoAppService.UpdateAsync(id, aluno);

            return Response(aluno);
        }

        /// <summary>
        /// Adiciona aluno
        /// </summary>
        /// <param name="aluno">Dados do aluno</param>
        /// <returns></returns>
        /// <remarks>
        /// Observações:<br /><br />
        /// A idade permitida no cadastro de alunos é de <b>5 a 15 anos</b>.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<AlunoViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAluno([FromBody] AdicionarAlunoViewModel aluno)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _alunoAppService.RegisterAsync(aluno);

            return Response(response);
        }

        /// <summary>
        /// Deleta aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAluno(long id)
        {
            if (!await _alunoAppService.AnyAsync(id))
                return NoContent();

            await _alunoAppService.RemoveAsync(id);

            return Response();
        }

    }
}
