using MockQueryable.Moq;
using Moq;
using PhidelisMatricula.Domain.Core.Notifications;
using PhidelisMatricula.Domain.Entities;
using PhidelisMatricula.Domain.Entities.Enumerables;
using PhidelisMatricula.Domain.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhidelisMatricula.Tests.Mocks
{
    public static class MockRepositories
    {
        public static Mock<IAlunoRepository> GetAlunoRepository()
        {
            var alunos = new List<Aluno>
            {
                new Aluno(
                    1,
                    "Teste 1",
                    10
                 ),
                new Aluno(
                    2,
                    "Teste 2",
                    11
                 ),
                new Aluno(
                    3,
                    "Teste 3",
                    12
                 ),
            };

            var mock = alunos.AsQueryable().BuildMock();
            var mockRepo = new Mock<IAlunoRepository>();

            mockRepo.Setup(r => r.GetAll()).Returns(mock.Object);
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<long>())).Returns((long id) =>
            {
                return Task.FromResult(alunos.FirstOrDefault(c => c.Id == id));
            });
            mockRepo.Setup(c => c.Add(It.IsAny<Aluno>())).Returns((Aluno aluno) =>
            {
                alunos.Add(aluno);
                return Task.CompletedTask;
            });

            return mockRepo;
        }

        public static Mock<IMatriculaRepository> GetMatriculaRepository()
        {
            var matriculas = new List<Matricula>
            {
                new Matricula(
                    1,
                    GetAlunoRepository().Object.GetAll().First().Id,
                    (int) StatusMatriculaEnumerable.Enum.Matriculado,
                    DateTime.Now.Year,
                    DateTime.Now,
                    ""
                 ),
                new Matricula(
                    2,
                    GetAlunoRepository().Object.GetAll().Skip(1).First().Id,
                    (int) StatusMatriculaEnumerable.Enum.Trancada,
                    DateTime.Now.Year + 1,
                    DateTime.Now,
                    ""
                 ),
                new Matricula(
                    3,
                    GetAlunoRepository().Object.GetAll().Skip(2).First().Id,
                    (int) StatusMatriculaEnumerable.Enum.Cancelada,
                    DateTime.Now.Year + 2,
                    DateTime.Now,
                    ""
                 ),
            };

            var mock = matriculas.AsQueryable().BuildMock();
            var mockRepo = new Mock<IMatriculaRepository>();

            mockRepo.Setup(r => r.GetAll()).Returns(mock.Object);
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<long>())).Returns((long id) =>
            {
                return Task.FromResult(matriculas.FirstOrDefault(c => c.Id == id));
            });
        
            mockRepo.Setup(c => c.Add(It.IsAny<Matricula>())).Returns((Matricula matricula) =>
            {
                matriculas.Add(matricula);
                return Task.CompletedTask;
            });

            return mockRepo;
        }
    }
}
