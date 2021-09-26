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
        public class ListaCursos : IRequest<List<Curso>> 
        {
            
        }


        public class Manejador : IRequestHandler<ListaCursos, List<Curso>>
        {
            private readonly CursosOnlineContext DbContextCursosOnline;

            public Manejador(CursosOnlineContext DbContextCursosOnline)
            {
                this.DbContextCursosOnline = DbContextCursosOnline;
            }

            public async Task<List<Curso>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var Cursos = await DbContextCursosOnline.Curso.ToListAsync();
                return Cursos;
            }
        }


    }
}
