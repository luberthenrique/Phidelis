using PhidelisMatricula.Domain.Entities;
using PhidelisMatricula.Domain.Entities.Enumerables;
using PhidelisMatricula.Domain.Validations;
using System;
using Xunit;

namespace PhidelisMatricula.Tests.Validations
{
    public class MatriculaValidationTests
    {
        private static int AnoAtual() => DateTime.Now.Year;
        private readonly DateTime dataAtual = DateTime.Now;

        public static readonly object[][] MatriculasValidas =
        {
            new object[] { new Matricula( 1, (int) StatusMatriculaEnumerable.Enum.Matriculado, DateTime.Now.Year, DateTime.Now, "")},
            new object[] { new Matricula( 1, (int) StatusMatriculaEnumerable.Enum.Trancada, DateTime.Now.Year, DateTime.Now, "")},
            new object[] { new Matricula( 1, (int) StatusMatriculaEnumerable.Enum.Cancelada, DateTime.Now.Year, DateTime.Now, "")},
            new object[] { new Matricula( 1, (int) StatusMatriculaEnumerable.Enum.Matriculado, DateTime.Now.Year + 5, DateTime.Now, "")},
        };

        public static readonly object[][] MatriculasInValidas =
        {
            // ID do aluno inválido
            new object[] { new Matricula( 0, (int) StatusMatriculaEnumerable.Enum.Matriculado, DateTime.Now.Year, DateTime.Now, "")},
            // Status que não existe
            new object[] { new Matricula( 1, 0, DateTime.Now.Year, DateTime.Now, "")},
            // Ano letivo menor que o atual
            new object[] { new Matricula( 1, (int)StatusMatriculaEnumerable.Enum.Matriculado, DateTime.Now.Year - 1, DateTime.Now, "")},
            // Ano letivo maior que o ano atual + 5 anos
            new object[] { new Matricula( 1, (int) StatusMatriculaEnumerable.Enum.Matriculado, DateTime.Now.Year + 6, DateTime.Now, "")},
        };

        [Theory]
        [MemberData(nameof(MatriculasValidas))]
        public void Validar_DeveSerValido_QuandoParametrosValidos(Matricula matricula)
        {
            var validationResult = new MatriculaValidation().Validate(matricula);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [MemberData(nameof(MatriculasInValidas))]
        public void Validar_DeveSerInValido_QuandoParametrosInvalidos(Matricula matricula)
        {
            var validationResult = new MatriculaValidation().Validate(matricula);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
        }
    }


}
