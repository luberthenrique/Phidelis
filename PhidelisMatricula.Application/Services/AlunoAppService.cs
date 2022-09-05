using MediatR;
using Microsoft.EntityFrameworkCore;
using PhidelisMatricula.Application.Interfaces;
using PhidelisMatricula.Application.ViewModels.Aluno;
using PhidelisMatricula.Core.Application.ViewModels;
using PhidelisMatricula.Domain.Core.Notifications;
using PhidelisMatricula.Domain.Entities;
using PhidelisMatricula.Domain.Entities.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhidelisMatricula.Application.Services
{
    public class AlunoAppService : BaseApplicationService, IAlunoAppService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoAppService(
            INotificationHandler<DomainNotification> notifications,
            IAlunoRepository alunoRepository
            ) : base(notifications)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<bool> AnyAsync(long id)
        {
            return await _alunoRepository.GetAll().AnyAsync(c => c.Id == id);
        }

        public void Dispose()
        {
            _alunoRepository.Dispose();
        }

        public async Task Truncate()
        {
            await _alunoRepository.Truncate();
        }

        public async Task<ConsultaPaginadaViewModel<AlunoViewModel>> GetAllAsync(int pagina, int quantidadeDadosPagina, string nome)
        {
            if (quantidadeDadosPagina > 100)
                _notifications.AddNotification("", "A quantidade máxima de dados a ser exibido por página é 100.");

            if (quantidadeDadosPagina < 1)
                _notifications.AddNotification("", "A quantidade de dados a ser exibido por página deve ser maior que 0.");

            if (pagina < 1)
                _notifications.AddNotification("", "A página deve ser maior que 0.");

            if (_notifications.HasNotifications())
                return null;

            var query = _alunoRepository
                .GetAll().
                Where(c => string.IsNullOrEmpty(nome) || c.Nome.ToLower().Contains(nome.ToLower()))
                .Select(a => new AlunoViewModel
            {
                Id = a.Id,
                Idade = a.Idade,
                Nome = a.Nome
            });

            return new ConsultaPaginadaViewModel<AlunoViewModel>
            {
                Dados = await query.Skip((pagina - 1) * quantidadeDadosPagina).Take(quantidadeDadosPagina).ToListAsync(),
                Total = await query.CountAsync(),
                Pagina = pagina,
                QuantidadePorPagina = quantidadeDadosPagina,
            };
        }

        public async Task<AlunoViewModel> GetByIdAsync(long id)
        {
            var aluno = await _alunoRepository.GetByIdAsync(id);

            if (aluno is null)
                return null;

            return new AlunoViewModel
            {
                Id = aluno.Id,
                Idade = aluno.Idade,
                Nome = aluno.Nome
            };
        }

        public async Task<AlunoViewModel> RegisterAsync(AdicionarAlunoViewModel alunoViewModel)
        {
            var aluno = new Aluno(
                alunoViewModel.Nome, 
                alunoViewModel.Idade
                );

            var validations = aluno.Validate();
            if (!validations.IsValid)
            {
                NotifyValidationErrors(validations, "AdicionarAluno");

                return null;
            }

            await _alunoRepository.Add(aluno);
            await _alunoRepository.SaveChangesAsync();

            return new AlunoViewModel
            {
                Id = aluno.Id,
                Idade = aluno.Idade,
                Nome = aluno.Nome
            };
        }

        public async Task RemoveAsync(long id)
        {
            await _alunoRepository.RemoveAsync(id);
            await _alunoRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(long id, AlterarAlunoViewModel alunoViewModel)
        {
            var aluno = await _alunoRepository.GetByIdAsync(id);
            aluno.Atualizar(
                alunoViewModel.Nome, 
                alunoViewModel.Idade
                );

            var validations = aluno.Validate();
            if (!validations.IsValid)
            {
                NotifyValidationErrors(validations, "AlterarAluno");

                return;
            }

            _alunoRepository.Update(aluno);
            await _alunoRepository.SaveChangesAsync();
        }
    }
}
