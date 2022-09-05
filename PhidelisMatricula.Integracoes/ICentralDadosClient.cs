using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhidelisMatricula.Integracoes
{
    public interface ICentralDadosClient
    {
        Task<List<string>> ObterNomes(int quantidade);
    }
}
