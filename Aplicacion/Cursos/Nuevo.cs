using MediatR;
using ProyectoCore.Dominio.Entidades;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime FechaPublicacion { get; set; }
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
                var curso = new Curso
                {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                cursosOnlineContext.Curso.Add(curso);
                var result = await cursosOnlineContext.SaveChangesAsync();

                if (result > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo implementar la funcion");
            }
        }

    }
}
