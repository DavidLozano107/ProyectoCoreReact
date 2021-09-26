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
    public class Eliminar
    {
        public class Ejecuta :IRequest
        {
            public int CursoId { get; set; }
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

                throw new Exception("El curso no existe");

            }
        }

    }
}
