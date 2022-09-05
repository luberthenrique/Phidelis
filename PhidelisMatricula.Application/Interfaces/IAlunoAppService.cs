using PhidelisMatricula.Application.ViewModels.Aluno;
using PhidelisMatricula.Core.Application.ViewModels;
using System;
using System.Threading.Tasks;

namespace PhidelisMatricula.Application.Interfaces
{
    public interface IAlunoAppService : IDisposable
    {       
        Task UpdateAsync(long id, AlterarAlunoViewModel alunoViewModel);
        Task<AlunoViewModel> RegisterAsync(AdicionarAlunoViewModel alunoViewModel);
        Task<AlunoViewModel> GetByIdAsync(long id);
        Task<ConsultaPaginadaViewModel<AlunoViewModel>> GetAllAsync(int pagina, int quantidadeDadosPagina, string nome);
        Task RemoveAsync(long id);
        Task<bool> AnyAsync(long id);
        Task Truncate();
    }
}
