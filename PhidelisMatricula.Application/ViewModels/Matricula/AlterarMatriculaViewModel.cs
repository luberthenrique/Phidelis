using System.ComponentModel.DataAnnotations;

namespace PhidelisMatricula.Application.ViewModels.Matricula
{
    public class AlterarMatriculaViewModel
    {        
        public int? Status { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int AnoLetivo { get; set; }
        [MaxLength(100, ErrorMessage = "O campo {0} deve conter até {1} caracteres.")]
        public string Observacao { get; set; }
    }
}
