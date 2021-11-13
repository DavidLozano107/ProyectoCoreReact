using Microsoft.AspNetCore.Mvc;
using ProyectoCore.Aplicacion.Seguridad;
using ProyectoCore.Dominio.Entidades;
using System.Threading.Tasks;

namespace ProyectoCore.API.Controllers
{

    public class UsuarioController : MyControllerBase
    {

        // http:localhost:44385/api/usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login([FromBody] Login.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }
    
    }
}
