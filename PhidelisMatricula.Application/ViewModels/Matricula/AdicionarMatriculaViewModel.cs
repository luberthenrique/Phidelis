using System.ComponentModel.DataAnnotations;

namespace PhidelisMatricula.Application.ViewModels.Matricula
{
    public class AdicionarMatriculaViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public long IdAluno { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int AnoLetivo { get; set; }
        [MaxLength(100, ErrorMessage = "O campo {0} deve conter até {1} caracteres.")]
        public string Observacao { get; set; }
    }
}
