using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProyectoCore.Aplicacion.ComentariosAp;
using System;
using System.Threading.Tasks;

namespace ProyectoCore.API.Controllers
{
    public class ComentarioController : MyControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear([FromBody] Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await Mediator.Send(new Eliminar.Ejecuta() { ComentarioId = id });
        }

    }
}
