using PhidelisMatricula.Application.ViewModels.Matricula;
using PhidelisMatricula.Core.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhidelisMatricula.Application.Interfaces
{
    public interface IMatriculaAppService : IDisposable
    {       
        Task UpdateAsync(long id, AlterarMatriculaViewModel matricula);
        Task<MatriculaViewModel> RegisterAsync(AdicionarMatriculaViewModel matricula);
        Task<MatriculaViewModel> GetByIdAsync(long id);
        Task<ConsultaPaginadaViewModel<MatriculaViewModel>> GetAllAsync(int pagina, int quantidadeDadosPagina, int? status, int? anoLetivo, string nomeAluno);
        Task RemoveAsync(long id);
        Task<bool> AnyAsync(long id);
        Task Truncate();
    }
}
