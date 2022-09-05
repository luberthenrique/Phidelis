using System.Collections.Generic;
using System.Linq;

namespace PhidelisMatricula.Domain.Entities.Enumerables
{
    public class StatusMatriculaEnumerable
    {
        public enum Enum
        {
            Matriculado = 1,
            Trancada = 2,
            Cancelada = 3,
        }

        public static List<int> GetList()
        {
            return Enum.GetValues(typeof(StatusMatriculaEnumerable.Enum)).Cast<int>().ToList();
        }
    }
}
