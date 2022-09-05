using PhidelisMatricula.Application.ViewModels.Aluno;
using System;
using System.ComponentModel.DataAnnotations;

namespace PhidelisMatricula.Application.ViewModels.Matricula
{
    public class MatriculaViewModel
    {
        public long Id { get; set; }
        public long IdAluno { get; set; }
        public string Codigo 
        { 
            get 
            {
                return IdAluno + Id.ToString("D3");
            } 
        }
        public int? Status { get; set; }
        public int AnoLetivo { get; set; }
        public DateTime DataMatricula { get; set; }
        public string Observacao { get; set; }

        public AlunoViewModel Aluno { get; set; }
    }
}
