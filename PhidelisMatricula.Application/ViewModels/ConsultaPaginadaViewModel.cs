using System.Collections.Generic;

namespace PhidelisMatricula.Core.Application.ViewModels
{
    public class ConsultaPaginadaViewModel<T>
    {
        public IList<T> Dados { get; set; }
        public int Pagina { get; set; }
        public int QuantidadePorPagina { get; set; }
        public int Total { get; set; }
    }
}
