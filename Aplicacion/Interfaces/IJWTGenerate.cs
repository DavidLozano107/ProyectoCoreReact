using ProyectoCore.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Contratos.Interfaces
{
    public interface IJWTGenerate
    {
        string CrearToken(Usuario usuario);
    }
}
