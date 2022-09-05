using Moq;
using PhidelisMatricula.Application.Services;
using PhidelisMatricula.Application.ViewModels.Matricula;
using PhidelisMatricula.Domain.Core.Notifications;
using PhidelisMatricula.Domain.Entities;
using PhidelisMatricula.Domain.Entities.Enumerables;
using PhidelisMatricula.Domain.Entities.Repository;
using PhidelisMatricula.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhidelisMatricula.Tests.Services
{
    public class MatriculaAppServiceTests
    {
        private readonly Mock<IMatriculaRepository> _mockMatriculaRepository;
        private readonly Mock<IAlunoRepository> _mockAlunoRepository;
        private readonly DomainNotificationHandler _domainNotifications;

        public MatriculaAppServiceTests()
        {
            _mockMatriculaRepository = MockRepositories.GetMatriculaRepository();
            _mockAlunoRepository = MockRepositories.GetAlunoRepository();

            _domainNotifications = new DomainNotificationHandler();
        }

        [Fact]
        public async Task Adicionar_Valido()
        {
            #region Arrange
            var aluno = _mockAlunoRepository.Object.GetAll().First();
            var matricula = new AdicionarMatriculaViewModel
            {
                IdAluno = aluno.Id,
                AnoLetivo = _mockMatriculaRepository.Object.GetAll().First(c => c.IdAluno == aluno.Id).AnoLetivo + 1,
                Observacao = "ABC"
            };
            #endregion

            #region Act
            var appService = new MatriculaAppService(
                _domainNotifications,
                _mockMatriculaRepository.Object,
                _mockAlunoRepository.Object);

            await appService.RegisterAsync(matricula);
            #endregion

            #region Assert
            Assert.False(_domainNotifications.HasNotifications());
            var matriculasAdicionadas = _mockMatriculaRepository.Object.GetAll().Where(c => c.IdAluno == matricula.IdAluno && c.AnoLetivo == matricula.AnoLetivo && c.Observacao == matricula.Observacao);
            Assert.Single(matriculasAdicionadas);
            #endregion
        }

        [Fact]
        public async Task Adicionar_Invalido_Aluno_Nao_Existe()
        {
            #region Arrange
            var matricula = new AdicionarMatriculaViewModel
            {
                IdAluno = 1000,
                AnoLetivo = DateTime.Now.Year,
                Observacao = "ABC"
            };
            #endregion

            #region Act
            var appService = new MatriculaAppService(
                _domainNotifications,
                _mockMatriculaRepository.Object,
                _mockAlunoRepository.Object);

            await appService.RegisterAsync(matricula);
            #endregion

            #region Assert
            Assert.True(_domainNotifications.HasNotifications());
            var matriculasAdicionadas = _mockMatriculaRepository.Object.GetAll().Where(c => c.IdAluno == matricula.IdAluno && c.AnoLetivo == matricula.AnoLetivo && c.Observacao == matricula.Observacao);
            Assert.Empty(matriculasAdicionadas);
            #endregion
        }


        [Fact]
        public async Task Adicionar_Invalido_Matricula_Ja_Existe_Para_Aluno_e_AnoLetivo()
        {
            #region Arrange
            var aluno = _mockAlunoRepository.Object.GetAll().First();
            var matricula = new AdicionarMatriculaViewModel
            {
                IdAluno = aluno.Id,
                AnoLetivo = _mockMatriculaRepository.Object.GetAll().First(c => c.IdAluno == aluno.Id).AnoLetivo,
                Observacao = "ABC"
            };
            #endregion

            #region Act
            var appService = new MatriculaAppService(
                _domainNotifications,
                _mockMatriculaRepository.Object,
                _mockAlunoRepository.Object);

            await appService.RegisterAsync(matricula);
            #endregion

            #region Assert
            Assert.True(_domainNotifications.HasNotifications());
            var matriculasAdicionadas = _mockMatriculaRepository.Object.GetAll().Where(c => c.IdAluno == matricula.IdAluno && c.AnoLetivo == matricula.AnoLetivo && c.Observacao == matricula.Observacao);
            Assert.Empty(matriculasAdicionadas);
            #endregion
        }

        [Fact]
        public async Task Alterar_Valido()
        {
            #region Arrange
            var idMatricula = 1;
            var matricula = new AlterarMatriculaViewModel
            {
                Status = (int) StatusMatriculaEnumerable.Enum.Cancelada,
                AnoLetivo = DateTime.Now.Year,
                Observacao = "ABC"
            };
            #endregion

            #region Act
            var appService = new MatriculaAppService(
                _domainNotifications,
                _mockMatriculaRepository.Object,
                _mockAlunoRepository.Object);

            await appService.UpdateAsync(idMatricula, matricula);
            #endregion

            #region Assert
            Assert.False(_domainNotifications.HasNotifications());
            var matriculasAdicionadas = _mockMatriculaRepository.Object.GetAll().Where(c => c.Id == idMatricula);
            Assert.Equal((int)StatusMatriculaEnumerable.Enum.Cancelada, matricula.Status);
            #endregion
        }

        [Fact]
        public async Task Alterar_Invalido_Status_Nao_Existe()
        {
            #region Arrange
            var idMatricula = 1;
            var matricula = new AlterarMatriculaViewModel
            {
                Status = (int)StatusMatriculaEnumerable.Enum.Cancelada + 5,
                AnoLetivo = DateTime.Now.Year,
                Observacao = "ABC"
            };
            #endregion

            #region Act
            var appService = new MatriculaAppService(
                _domainNotifications,
                _mockMatriculaRepository.Object,
                _mockAlunoRepository.Object);

            await appService.UpdateAsync(idMatricula, matricula);
            #endregion

            #region Assert
            Assert.True(_domainNotifications.HasNotifications());
            #endregion
        }
    }
}
