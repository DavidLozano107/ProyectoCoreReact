using AutoMapper;
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

namespace ProyectoCore.Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDTO>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDTO>
        {
            private readonly CursosOnlineContext CursosOnlineContext;
            private readonly IMapper mapper;

            public Manejador(CursosOnlineContext CursosOnlineContext, IMapper mapper)
            {
                this.CursosOnlineContext = CursosOnlineContext;
                this.mapper = mapper;
            }
            public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var Curso = await CursosOnlineContext.Curso
                    .Include(x => x.Comentarios)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructorLink)
                    .ThenInclude(x => x.Instructor)
                    .FirstOrDefaultAsync(x => x.CursoId == request.Id);

                if (Curso==null)
                {
                    throw new ManejadorExepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso" });
                }

                var CursoDTO = mapper.Map<Curso, CursoDTO>(Curso);
                return CursoDTO;
            }
        }


    }
}
