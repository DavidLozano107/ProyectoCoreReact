using FluentValidation;
using MediatR;
using ProyectoCore.Aplicacion.ManejadorError;
using ProyectoCore.Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta: IRequest
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Grado { get; set; }
        }

        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x=>x.Nombre).NotEmpty();
                RuleFor(x=>x.Apellidos).NotEmpty();
                RuleFor(x=>x.Grado).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor instructorRepositorio;

            public Manejador(IInstructor InstructorRepositorio)
            {
                instructorRepositorio = InstructorRepositorio;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                InstructorModel instructor = new()
                {
                    Nombre = request.Nombre,
                    Apellidos = request.Apellidos,
                    Grado = request.Grado,
                    FechaCreacion = DateTime.UtcNow
                };

                var response = await instructorRepositorio.NuevoInstructor(instructor);

                if (response > 0)
                {
                    return Unit.Value;
                }
                
                throw new Exception("No se pudo insertar el instructor");
                
            }
        }

    }
}
