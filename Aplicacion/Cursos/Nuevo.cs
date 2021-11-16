using FluentValidation;
using MediatR;
using ProyectoCore.Dominio.Entidades;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal Precio { get; set; }
            public decimal PrecioPromocion { get; set; }

        }

        //Fluent Validation
        public class EjecutaValidacion:AbstractValidator<Ejecuta> 
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty().WithMessage("El titulo no puede ser vacio");
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.Precio).NotEmpty();
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
                var curso = new Curso
                {
                    CursoId = Guid.NewGuid(),
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                
                cursosOnlineContext.Curso.Add(curso);

                if (request.ListaInstructor!= null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoId = curso.CursoId,
                            InstructorId = id
                        };
                        cursosOnlineContext.CursoInstructor.Add(cursoInstructor);
                    }
                }

                //Agregar precio del curso
                var PrecioEntity = new Precio()
                {
                    PrecioId = Guid.NewGuid(),
                    CursoId = curso.CursoId,
                    PrecioActual = request.Precio,
                    Promocion = request.PrecioPromocion
                };

                cursosOnlineContext.Precio.Add(PrecioEntity);

                var result = await cursosOnlineContext.SaveChangesAsync();

                if (result > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el curso");
            }
        }

    }
}
