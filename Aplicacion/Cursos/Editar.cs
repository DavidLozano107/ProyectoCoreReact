using MediatR;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public int CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext cursosOnlineContext;

            public Manejador(CursosOnlineContext cursosOnlineContext)
            {
                this.cursosOnlineContext = cursosOnlineContext;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await cursosOnlineContext.Curso.FindAsync(request.CursoId);

                if (curso == null)
                {
                    throw new Exception("El curso no existe");
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                cursosOnlineContext.Curso.Update(curso);
                var response = await cursosOnlineContext.SaveChangesAsync();

                if (response> 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambios en el curso");


            }
        }

    }
}
