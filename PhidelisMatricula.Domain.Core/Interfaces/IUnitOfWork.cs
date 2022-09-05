using System;
using System.Threading.Tasks;

namespace PhidelisMatricula.Domain.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}
