using PhidelisMatricula.Domain.Core.Interfaces;
using System.Threading.Tasks;

namespace PhidelisMatricula.Domain.Entities.Repository
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        Task Truncate();
    }
}
