using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProyectoCore.Aplicacion.ManejadorError;
using ProyectoCore.Dominio.Entidades;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.ComentariosAp
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid ComentarioId { get; set; }
        }

        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.ComentarioId).NotEmpty();   
            }
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
                var comentario = await cursosOnlineContext.Comentario.FirstOrDefaultAsync(x => x.ComentarioId == request.ComentarioId);

                if (comentario == null)
                {
                     new ManejadorExepcion(HttpStatusCode.NotFound,new {mensaje="Error"});
                }

                cursosOnlineContext.Comentario.Remove(comentario);
                var resultado = await cursosOnlineContext.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("Error al eliminar el comentario");

            }
        }

    }
}
