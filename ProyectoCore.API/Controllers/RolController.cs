using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoCore.Aplicacion.Seguridad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.API.Controllers
{

    public class RolController : MyControllerBase
    {
        [HttpPost("crear")] 
        public async Task<ActionResult<Unit>> Crear(RolNuevo.Ejecuta data) => await Mediator.Send(data);

        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Delete(EliminarRol.Ejecuta data) => await Mediator.Send(data);


        [HttpGet]
        public async Task<ActionResult<List<IdentityRole>>> Get()
        {
            return await Mediator.Send(new Consulta.Ejecuta() { });
        }
        
        [HttpGet("ConsultaId/{id}")]
        public async Task<ActionResult<IdentityRole>> Get(string id)
        {
            return await Mediator.Send(new ConsultaId.Ejectuta() { Id = id });
        }


        [HttpPost("agregarRoleUsuario")]
        public async Task<ActionResult<Unit>> AgregarRoleUsuario([FromBody] UsuarioRolAgregar.Ejecuta ejecuta)
        {
            return await Mediator.Send(ejecuta);
        }

        [HttpDelete("eliminarRoleUsuario")]
        public async Task<ActionResult<Unit>> EliminarRolUsuario([FromBody] UsuarioRolEliminar.Ejecutar data)
        {
            return await Mediator.Send(data);
        }

        [HttpGet("ObtenerRolesByUsuario/{UserName}")]
        public async Task<ActionResult<List<string>>> GetRolesByUserName(string UserName)
        {
            return await Mediator.Send(new ObtenerRolesByUsuario.Ejecuta() { UserName = UserName });
        }

    }
}
