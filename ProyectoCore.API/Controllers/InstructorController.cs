using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoCore.Aplicacion.Instructores;
using ProyectoCore.Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.API.Controllers
{
    [Authorize]
    public class InstructorController : MyControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores()
        {
            var InstrunctoresLista = await Mediator.Send(new Consulta.Ejecuta());
            return InstrunctoresLista;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> ObtenerInstructores(Guid id)
        {
            var Instrunctor = await Mediator.Send(new ConsultaId.Ejecuta{ InstructorId = id});
            return Instrunctor;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid id, [FromBody] Editar.Ejecuta data)
        {
            data.InstructorId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid Id)
        {
            return await Mediator.Send(new Elimina.Ejecuta { InstructorId = Id});
        }
    }
}
    