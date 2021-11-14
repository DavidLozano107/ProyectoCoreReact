using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoCore.Aplicacion.Seguridad;
using ProyectoCore.Dominio.Entidades;
using System.Threading.Tasks;

namespace ProyectoCore.API.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : MyControllerBase
    {
        // http:localhost:44385/api/usuario/login
        [HttpPost("login")]
       
        public async Task<ActionResult<UsuarioData>> Login([FromBody] Login.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        // http:localhost:44385/api/usuario/register
        [HttpPost("register")]
        public async Task<UsuarioData> Register([FromBody] Registrar.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            return await Mediator.Send(new UsuarioActual.Ejecuta());
        }

    }
}
