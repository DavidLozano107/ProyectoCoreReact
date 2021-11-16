using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>> 
        {
            
        }


        public class Manejador : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            private readonly CursosOnlineContext DbContextCursosOnline;
            private readonly IMapper mapper;

            public Manejador(CursosOnlineContext DbContextCursosOnline, IMapper mapper)
            {
                this.DbContextCursosOnline = DbContextCursosOnline;
                this.mapper = mapper;
            }

            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var Cursos = await DbContextCursosOnline.Curso.
                     Include(x=>x.Comentarios)
                    .Include(x=>x.PrecioPromocion)
                    .Include(x => x.InstructorLink)
                    .ThenInclude(x => x.Instructor)
                    .ToListAsync();

                var cursoDTO = mapper.Map<List<Curso>, List<CursoDTO>>(Cursos);

                return cursoDTO;
            }
        }


    }
}
