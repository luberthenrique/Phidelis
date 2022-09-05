using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhidelisMatricula.Integracoes
{
    public class CentralDadosClient : ICentralDadosClient
    {
        public Task<List<string>> ObterNomes(int quantidade)
        {
            throw new NotImplementedException();
        }
    }
}
