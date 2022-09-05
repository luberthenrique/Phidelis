using FluentValidation;
using PhidelisMatricula.Domain.Entities;

namespace PhidelisMatricula.Domain.Validations
{
    public class AlunoValidation : AbstractValidator<Aluno>
    {
        public AlunoValidation()
        {
            ValidarNome();
            ValidarIdade();
        }

        protected void ValidarNome()
        {
            RuleFor(c => c.Nome)
                .NotNull()
                .NotEmpty()
                .WithMessage($"O nome é obrigatorio.")
                .Length(2, 100)
                    .WithMessage("O nome deve conter entre 2 e 50 caracteres.");
        }

        protected void ValidarIdade()
        {
            RuleFor(c => c.Idade)
                .NotNull()
                .NotEmpty()
                .WithMessage($"A idade é obrigatoria.")
                .InclusiveBetween(5, 15)
                .WithMessage($"O aluno deve conter entre 5 e 15 anos.");
        }
    }
}
