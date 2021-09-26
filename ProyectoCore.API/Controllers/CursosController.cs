using MediatR;
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
    public class CursosController : ControllerBase
    {
        private readonly IMediator mediator;

        public CursosController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Curso>>> GetCursos()
        {
            var cursos = await mediator.Send(new Consulta.ListaCursos());
            return cursos;

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Curso>>GetCursoById(int id)
        {
            var curso = await mediator.Send(new ConsultaId.CursoUnico { Id=id});
            return curso;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CrearCurso([FromBody] Nuevo.Ejecuta curso)
        {
            var response = await mediator.Send(curso);
            return response;

        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<Unit>> EditarCurso(int id, [FromBody] Editar.Ejecuta data)
        {
            data.CursoId = id;
            var response = await mediator.Send(data);
            return response;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Unit>> EliminarCurso(int id)
        {
            var response = await mediator.Send(new Eliminar.Ejecuta { CursoId = id});
            return response;

        }


    }
}
