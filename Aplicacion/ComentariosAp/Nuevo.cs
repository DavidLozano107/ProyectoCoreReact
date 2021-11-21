using FluentValidation;
using MediatR;
using ProyectoCore.Dominio.Entidades;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.ComentariosAp
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Almuno { get; set; }
            public int Puntaje { get; set; }
            public string Comentario { get; set; }
            public Guid CursoId { get; set; }
        }


        public class Validacion :AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.Almuno).NotEmpty();
                RuleFor(x => x.Comentario).NotEmpty();
                RuleFor(x => x.CursoId).NotEmpty();
            }
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
                var comentario = new Comentario()
                {
                    ComentarioId = Guid.NewGuid(),
                    Alumno = request.Almuno,
                    ComentarioTexto = request.Comentario,
                    CursoId = request.CursoId,
                    Puntaje = request.Puntaje,
                    FechaCreacion = DateTime.UtcNow
                };

                cursosOnlineContext.Comentario.Add(comentario);

                var resultado = await cursosOnlineContext.SaveChangesAsync();
                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("Error a la hora de crear un comentario");
            }
        }

    }
}
