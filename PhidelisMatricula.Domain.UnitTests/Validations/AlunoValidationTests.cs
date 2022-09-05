using PhidelisMatricula.Domain.Entities;
using PhidelisMatricula.Domain.Validations;
using Xunit;

namespace PhidelisMatricula.Tests.Validations
{
    public class AlunoValidationTests
    {
        public static readonly object[][] AlunosValidos =
        {
            // Tamanho mínimo nome e idade mínima válidos
            new object[] { new Aluno("AB", 5) },
            // Idade máxima válida
            new object[] { new Aluno("Teste 1", 15) },
            // Tamanho máximo nome valido
            new object[] { new Aluno("Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio", 5) }
        };

        public static readonly object[][] AlunosInValidos =
        {
            // Idade menor que a mínima
            new object[] { new Aluno("Teste 1", 4) },
             // Idade maior que a máxima
            new object[] { new Aluno("Teste 1", 16) },
            // Tamanho do nome menor que o permitido
            new object[] { new Aluno("A", 8) },
            // Tamanho do nome maior que o permitido
            new object[] { new Aluno("Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio ", 5) },
        };

        [Theory]
        [MemberData(nameof(AlunosValidos))]
        public void Validar_Deve_Ser_Valido_QuandoParametrosValidos(Aluno aluno)
        {
            var validationResult = new AlunoValidation().Validate(aluno);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [MemberData(nameof(AlunosInValidos))]
        public void Validar_Deve_Ser_Invalido_QuandoParametrosInvalidos(Aluno aluno)
        {
            var validationResult = new AlunoValidation().Validate(aluno);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
        }
    }


}
