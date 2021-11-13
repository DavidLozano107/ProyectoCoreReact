using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.ManejadorError
{
    public class ManejadorExepcion : Exception
    {
        public readonly HttpStatusCode Codigo;
        public readonly object Errores;

        public ManejadorExepcion(HttpStatusCode Codigo, object Errores = null)
        {
            this.Codigo = Codigo;
            this.Errores = Errores;
        }
    }
}
