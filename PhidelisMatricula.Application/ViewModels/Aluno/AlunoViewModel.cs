using System.ComponentModel.DataAnnotations;

namespace PhidelisMatricula.Application.ViewModels.Aluno
{
    public class AlunoViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo {0} deve conter até {1} caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Idade { get; set; }
    }
}
