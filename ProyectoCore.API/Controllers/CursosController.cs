using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoCore.Aplicacion.Cursos;
using ProyectoCore.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : MyControllerBase
    {
       
        [HttpGet]
        public async Task<ActionResult<List<CursoDTO>>> GetCursos()
        {
            var cursos = await Mediator.Send(new Consulta.ListaCursos());
            return cursos;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>>GetCursoById(Guid id)
        {
            var curso = await Mediator.Send(new ConsultaId.CursoUnico { Id=id});
            return curso;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CrearCurso([FromBody] Nuevo.Ejecuta curso)
        {
            var response = await Mediator.Send(curso);
            return response;

        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> EditarCurso(Guid id, [FromBody] Editar.Ejecuta data)
        {
            data.CursoId = id;
            var response = await Mediator.Send(data);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> EliminarCurso(Guid id)
        {
            var response = await Mediator.Send(new Eliminar.Ejecuta { CursoId = id});
            return response;

        }


    }
}
