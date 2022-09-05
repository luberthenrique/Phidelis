using FluentValidation.Results;
using PhidelisMatricula.Domain.Core.Models;
using PhidelisMatricula.Domain.Validations;
using System.Collections.Generic;

namespace PhidelisMatricula.Domain.Entities
{
    public class Aluno : Entity
    {
        protected Aluno() 
        { 
        }

        public Aluno(
            string nome,
            int idade)
        {
            Nome = nome;
            Idade = idade;
        }

        public Aluno(
            long id,
            string nome, 
            int idade)
        {
            Id = id;
            Nome = nome;
            Idade = idade;
        }

        public string Nome { get; private set; }
        public int Idade { get; private set; }

        public virtual ICollection<Matricula> Matriculas { get; private set; }

        public void Atualizar(string nome, int idade)
        {
            Nome = nome;
            Idade = idade;
        }

        public ValidationResult Validate()
        {
            return new AlunoValidation().Validate(this);
        }

    }
}
