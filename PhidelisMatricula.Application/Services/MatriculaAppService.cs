using MediatR;
using Microsoft.EntityFrameworkCore;
using PhidelisMatricula.Application.Interfaces;
using PhidelisMatricula.Application.ViewModels.Aluno;
using PhidelisMatricula.Application.ViewModels.Matricula;
using PhidelisMatricula.Core.Application.ViewModels;
using PhidelisMatricula.Domain.Core.Notifications;
using PhidelisMatricula.Domain.Entities;
using PhidelisMatricula.Domain.Entities.Enumerables;
using PhidelisMatricula.Domain.Entities.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhidelisMatricula.Application.Services
{
    public class MatriculaAppService : BaseApplicationService, IMatriculaAppService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;

        public MatriculaAppService(
            INotificationHandler<DomainNotification> notifications,
            IMatriculaRepository matriculaRepository,
            IAlunoRepository alunoRepository
            ) : base(notifications)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
        }

        public async Task<bool> AnyAsync(long id)
        {
            return await _matriculaRepository.GetAll().AnyAsync(c => c.Id == id);
        }

        public void Dispose()
        {
            _matriculaRepository.Dispose();
        }

        public async Task Truncate()
        {
            await _matriculaRepository.Truncate();
        }

        public async Task<ConsultaPaginadaViewModel<MatriculaViewModel>> GetAllAsync(int pagina, int quantidadeDadosPagina, int? status, int? anoLetivo, string nomeAluno)
        {
            if (quantidadeDadosPagina > 100)
                _notifications.AddNotification("", "A quantidade máxima de dados a ser exibido por página é 100.");

            if (quantidadeDadosPagina < 1)
                _notifications.AddNotification("", "A quantidade de dados a ser exibido por página deve ser maior que 0.");

            if (pagina < 1)
                _notifications.AddNotification("", "A página deve ser maior que 0.");

            if (status!= null && !StatusMatriculaEnumerable.GetList().Contains(status.Value))
                _notifications.AddNotification("", "O status não existe no sistema.");

            if (_notifications.HasNotifications())
                return null;

            var query = _matriculaRepository.GetAll().Include(m => m.Aluno)
                .Where(m =>
                    (status == null || m.Status == status) &&
                    (anoLetivo == null || m.AnoLetivo == anoLetivo) &&
                    (string.IsNullOrEmpty(nomeAluno) || m.Aluno.Nome.ToLower().Contains(nomeAluno.ToLower()))
                )
                .Select(m => new MatriculaViewModel
                {
                    Id = m.Id,
                    IdAluno = m.IdAluno,
                    Status = m.Status,
                    AnoLetivo = m.AnoLetivo,
                    DataMatricula = m.DataMatricula,
                    Observacao = m.Observacao,
                    Aluno = new AlunoViewModel
                    {
                        Id = m.Aluno.Id,
                        Nome = m.Aluno.Nome,
                        Idade = m.Aluno.Idade
                    }
                });


            return new ConsultaPaginadaViewModel<MatriculaViewModel>
            {
                Dados = await query.OrderByDescending(c => c.DataMatricula).Skip((pagina - 1) * quantidadeDadosPagina).Take(quantidadeDadosPagina).ToListAsync(),
                Total = await query.CountAsync(),
                Pagina = pagina,
                QuantidadePorPagina = quantidadeDadosPagina,
            };
        }

        public async Task<MatriculaViewModel> GetByIdAsync(long id)
        {
            var matricula = await _matriculaRepository.GetAll().Include(c => c.Aluno).FirstOrDefaultAsync(c => c.Id == id);

            if (matricula is null)
                return null;

            return new MatriculaViewModel
            {
                Id = matricula.Id,
                IdAluno = matricula.IdAluno,
                Status = matricula.Status,
                AnoLetivo = matricula.AnoLetivo,
                DataMatricula = matricula.DataMatricula,
                Aluno = new AlunoViewModel
                {
                    Id = matricula.Aluno.Id,
                    Nome = matricula.Aluno.Nome,
                    Idade = matricula.Aluno.Idade
                }
            };
        }

        public async Task<MatriculaViewModel> RegisterAsync(AdicionarMatriculaViewModel matriculaViewModel)
        {
            var matricula = new Matricula(
                matriculaViewModel.IdAluno,
                (int)StatusMatriculaEnumerable.Enum.Matriculado, 
                matriculaViewModel.AnoLetivo, 
                System.DateTime.Now,
                matriculaViewModel.Observacao);

            var validations = matricula.Validate();
            if (!validations.IsValid)
            {
                NotifyValidationErrors(validations, "AdicionarMatricula");

                return null;
            }

            if (!await _alunoRepository.GetAll().AnyAsync(c => c.Id == matricula.IdAluno))
            {
                _notifications.AddNotification("", "Aluno não existe no sistema");
                return null;
            }

            var existeMatriculaAtivaParaAluno = await _matriculaRepository.GetAll().AnyAsync(c => c.IdAluno == matricula.IdAluno && c.AnoLetivo == matricula.AnoLetivo && c.Status == (int)StatusMatriculaEnumerable.Enum.Matriculado);
            if (existeMatriculaAtivaParaAluno)
            {
                _notifications.AddNotification("", $"Já existe uma matrícula ativa com o ano de {matricula.AnoLetivo} para o aluno informado.");
                return null;
            }

            await _matriculaRepository.Add(matricula);
            await _matriculaRepository.SaveChangesAsync();

            return new MatriculaViewModel
            {
                Id = matricula.Id,
                AnoLetivo = matricula.AnoLetivo,
                DataMatricula = matricula.DataMatricula,
                Status = matricula.Status,
                IdAluno = matricula.IdAluno,
                Observacao = matricula.Observacao
            };
        }

        public async Task RemoveAsync(long id)
        {
            await _matriculaRepository.RemoveAsync(id);
            await _matriculaRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(long id, AlterarMatriculaViewModel matriculaViewModel)
        {
            var matricula = await _matriculaRepository.GetByIdAsync(id);

            matricula.Atualizar(
                matriculaViewModel.Status ?? matricula.Status, 
                matriculaViewModel.AnoLetivo, 
                matriculaViewModel.Observacao);

            var validations = matricula.Validate();
            if (!validations.IsValid)
            {
                NotifyValidationErrors(validations, "AdicionarMatricula");

                return;
            }

            var existeMatriculaAtivaParaAluno = await _matriculaRepository.GetAll().AnyAsync(c => c.Id != matricula.Id && c.IdAluno == matricula.IdAluno && c.AnoLetivo == matricula.AnoLetivo && c.Status == (int)StatusMatriculaEnumerable.Enum.Matriculado);
            if (matriculaViewModel.Status == (int)StatusMatriculaEnumerable.Enum.Matriculado &&  existeMatriculaAtivaParaAluno)
            {
                _notifications.AddNotification("", $"Já existe uma matrícula ativa com o ano de {matricula.AnoLetivo} para o aluno informado.");
                return;
            }

            _matriculaRepository.Update(matricula);
            await _matriculaRepository.SaveChangesAsync();
        }
    }
}
