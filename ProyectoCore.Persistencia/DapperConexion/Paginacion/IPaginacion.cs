using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Persistencia.DapperConexion.Paginacion
{
    public interface IPaginacion
    {
        Task<PaginacionModel> DevolverPaginacion(
            string NombreProcedimiento, 
            int NumeroPagina, 
            int CantidadPagina, 
            IDictionary<string,object> parametrosFiltro, 
            string ordenamientoColumna);
    }
}
