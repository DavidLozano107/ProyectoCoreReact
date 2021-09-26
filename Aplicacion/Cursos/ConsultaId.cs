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
    public class ConsultaId
    {
        public class CursoUnico : IRequest<Curso>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, Curso>
        {
            private readonly CursosOnlineContext CursosOnlineContext;

            public Manejador(CursosOnlineContext CursosOnlineContext)
            {
                this.CursosOnlineContext = CursosOnlineContext;
            }
            public async Task<Curso> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var Curso = await CursosOnlineContext.Curso.FindAsync(request.Id);
                return Curso;
            }
        }


    }
}
