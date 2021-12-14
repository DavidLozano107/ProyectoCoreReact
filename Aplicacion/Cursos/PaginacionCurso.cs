using FluentValidation;
using MediatR;
using ProyectoCore.Persistencia.DapperConexion.Paginacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Cursos
{
    public class PaginacionCurso
    {
        public class Ejecuta : IRequest<PaginacionModel>
        {
            //Filtro del curso. 
            public string Titulo { get; set; }
            public int NumeroPagina { get; set; }
            public int CantidadPagina { get; set; }
        }

        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.NumeroPagina).NotEmpty();
                RuleFor(x => x.CantidadPagina).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, PaginacionModel>
        {
            private readonly IPaginacion paginacion;

            public Manejador(IPaginacion paginacion)
            {
                this.paginacion = paginacion;
            }

            public async Task<PaginacionModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var storedProcedure = "usp_obtener_curso_Paginacion";
                var ordenamientoColumna = "Titulo";

                var Parametros = new Dictionary<string,object>();
                Parametros.Add("NombreCurso",request.Titulo);

                return await paginacion.DevolverPaginacion(storedProcedure, request.NumeroPagina, request.CantidadPagina, Parametros, ordenamientoColumna);
            }
        }

    }
}
