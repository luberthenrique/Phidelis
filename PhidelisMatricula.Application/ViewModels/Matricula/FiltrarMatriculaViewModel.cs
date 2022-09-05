using System.ComponentModel.DataAnnotations;

namespace PhidelisMatricula.Application.ViewModels.Matricula
{
    public class FiltrarMatriculaViewModel
    {
        /// <summary>
        /// Teste
        /// </summary>
        [Display(Name ="Status")]
        public int? Status { get; set; }
        [Display(Name = "Ano Letivo")]
        public int? AnoLetivo { get; set; }
        [Display(Name = "Nome Aluno")]
        public string NomeAluno { get; set; }
    }
}
