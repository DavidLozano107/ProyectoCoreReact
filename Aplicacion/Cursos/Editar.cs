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

namespace ProyectoCore.Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> listaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? PrecioPromocion { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
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
                var curso = await cursosOnlineContext.Curso.FindAsync(request.CursoId);

                if (curso == null)
                {
                    throw new ManejadorExepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso" });
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                //Actualiza el precio o lo insertamos.
                if (request.Precio != null || request.PrecioPromocion != null)
                {
                    var precioEntidad = await cursosOnlineContext.Precio.FirstOrDefaultAsync(x => x.CursoId == request.CursoId);

                    if (precioEntidad != null)
                    {
                        precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                        precioEntidad.Promocion = request.PrecioPromocion?? precioEntidad.Promocion;
                        cursosOnlineContext.Precio.Update(precioEntidad);
                    }
                    else
                    {
                        precioEntidad = new Precio()
                        {
                            CursoId = request.CursoId,
                            PrecioActual = request.Precio ?? 0,
                            Promocion = request.PrecioPromocion ?? 0
                        };

                        await cursosOnlineContext.Precio.AddAsync(precioEntidad);
                    }

                }


                //Eliminamos los instructores del curso, para agregarlos despues. 

                if (request.listaInstructor != null)
                {
                    if (request.listaInstructor.Count>0)
                    {
                        //Elimina los instructores de cursoInstructor del curso a actualizar. 
                        var instructoresDB = await cursosOnlineContext.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToListAsync();
                        foreach (var InstructorCurso in instructoresDB)
                        {
                            cursosOnlineContext.CursoInstructor.Remove(InstructorCurso);
                        }

                        //Agrega instructores que provienen del cliente. 
                        foreach (var InstructorId in request.listaInstructor)
                        {
                            cursosOnlineContext.CursoInstructor.Add(new CursoInstructor() { CursoId = request.CursoId, InstructorId = InstructorId });
                        }

                    }    
                }

           


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
