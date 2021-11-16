using MediatR;
using Microsoft.EntityFrameworkCore;
using ProyectoCore.Aplicacion.ManejadorError;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta :IRequest
        {
            public Guid CursoId { get; set; }
        }

        public class Manjeador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext cursosOnlineContext;

            public Manjeador(CursosOnlineContext cursosOnlineContext)
            {
                this.cursosOnlineContext = cursosOnlineContext;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await cursosOnlineContext.Curso.FindAsync(request.CursoId);

                var CursoInstructores = cursosOnlineContext.CursoInstructor.Where(x => x.CursoId == request.CursoId);

                if (CursoInstructores.Any())
                {
                    foreach (var CursoInstructor in CursoInstructores)
                    {
                        cursosOnlineContext.CursoInstructor.Remove(CursoInstructor);
                    }
                }

                //Eliminar comentarios
                var ComentariosCurso = cursosOnlineContext.Comentario.Where(x => x.CursoId == request.CursoId).ToList();
                
                if (ComentariosCurso.Any())
                {
                    foreach (var Comentario in ComentariosCurso)
                    {
                        cursosOnlineContext.Comentario.Remove(Comentario);
                    }
                }

                //Precio
                var PrecioDB = await cursosOnlineContext.Precio.FirstOrDefaultAsync(x => x.CursoId == request.CursoId);
                if (PrecioDB != null)
                {
                    cursosOnlineContext.Precio.Remove(PrecioDB);
                }

                if (curso != null)
                {
                    
                    cursosOnlineContext.Curso.Remove(curso);

                    var response = await cursosOnlineContext.SaveChangesAsync();
                    if (response > 0)
                    {
                        return Unit.Value;
                    }
                    throw new Exception("No se pudo eliminar");
                }

                //throw new Exception("El curso no existe");
                throw new ManejadorExepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso"});

            }
        }

    }
}
