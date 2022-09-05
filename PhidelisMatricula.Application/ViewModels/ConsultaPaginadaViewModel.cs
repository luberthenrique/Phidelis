using System.Collections.Generic;

namespace PhidelisMatricula.Core.Application.ViewModels
{
    public class ConsultaPaginadaViewModel<T>
    {        
        public int Pagina { get; set; }
        public int QuantidadePorPagina { get; set; }
        public int Total { get; set; }
        public IList<T> Dados { get; set; }
    }
}
