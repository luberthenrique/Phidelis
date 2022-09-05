using FluentValidation;
using PhidelisMatricula.Domain.Entities;
using PhidelisMatricula.Domain.Entities.Enumerables;
using System;

namespace PhidelisMatricula.Domain.Validations
{
    public class MatriculaValidation : AbstractValidator<Matricula>
    {
        public MatriculaValidation()
        {
            ValidarIdAluno();
            ValidarAnoLetivo();
            ValidarNome();
            ValidarStatus();
        }

        protected void ValidarIdAluno()
        {
            RuleFor(c => c.IdAluno)
                .NotEmpty()
                .WithMessage($"O aluno é obrigatorio.");
        }

        protected void ValidarAnoLetivo()
        {
            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage($"O ano letivo é obrigatorio.")
                .GreaterThanOrEqualTo(DateTime.Now.Year)
                .WithMessage($"O ano letivo deve ser maior ou igual a {DateTime.Now.Year}.")
                .LessThanOrEqualTo(DateTime.Now.Year + 5)
                .WithMessage($"O ano letivo não pode ser superior a {DateTime.Now.Year + 5}.");
        }

        protected void ValidarNome()
        {
            RuleFor(c => c.Observacao)
                .MaximumLength(300)
                    .WithMessage("A observação deve conter no máximo 300 caracteres.");
        }

        protected void ValidarStatus()
        {
            RuleFor(c => c.Status)
                .NotNull()
                .WithMessage($"O status é obrigatorio.")
                .Custom((value, context) =>
                {
                    if (!StatusMatriculaEnumerable.GetList().Contains(value))
                        context.AddFailure("O status não existe no sistema.");
                });

        }
    }
}
