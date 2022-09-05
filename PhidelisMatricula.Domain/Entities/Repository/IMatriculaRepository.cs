using PhidelisMatricula.Domain.Core.Interfaces;
using System.Threading.Tasks;

namespace PhidelisMatricula.Domain.Entities.Repository
{
    public interface IMatriculaRepository : IRepository<Matricula>
    {
        Task Truncate();
    }
}
