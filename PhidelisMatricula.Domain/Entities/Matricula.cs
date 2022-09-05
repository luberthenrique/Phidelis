using FluentValidation.Results;
using PhidelisMatricula.Domain.Core.Models;
using PhidelisMatricula.Domain.Validations;
using System;

namespace PhidelisMatricula.Domain.Entities
{
    public class Matricula : Entity
    {
        protected Matricula()
        {

        }

        public Matricula(
            long idAluno,
            int status,
            int anoLetivo,
            DateTime dataMatricula,
            string observacao)
        {
            IdAluno = idAluno;
            Status = status;
            AnoLetivo = anoLetivo;
            DataMatricula = dataMatricula;
            Observacao = observacao;
        }
        public Matricula(
            long id,
            long idAluno, 
            int status,
            int anoLetivo,
            DateTime dataMatricula,
            string observacao)
        {
            Id = id;
            IdAluno = idAluno;
            Status = status;
            AnoLetivo = anoLetivo;
            DataMatricula = dataMatricula;
            Observacao = observacao;
        }

        public long IdAluno { get; private set; }
        public int Status { get; private set; }
        public int AnoLetivo { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public string Observacao { get; private set; }

        public Aluno Aluno { get; private set; }


        public void Atualizar(int status, int anoLetivo, string observacao)
        {
            Status = status;
            AnoLetivo = anoLetivo;
            Observacao = observacao;
        }

        public ValidationResult Validate()
        {
            return new MatriculaValidation().Validate(this);
        }
    }
}
