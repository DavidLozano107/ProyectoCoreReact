using FluentValidation;
using MediatR;
using ProyectoCore.Dominio.Entidades;
using ProyectoCore.Persistencia;
using ProyectoCore.Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Instructores
{
    public class Editar 
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Titulo { get; set; }
        }

        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Titulo).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor instructor;
            private readonly CursosOnlineContext cursosOnlineContext;

            public Manejador(IInstructor instructor,CursosOnlineContext cursosOnlineContext)
            {
                this.instructor = instructor;
                this.cursosOnlineContext = cursosOnlineContext;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var result = await instructor.EditarInstructor(request.InstructorId, request.Nombre, request.Apellidos, request.Titulo);

                if (result > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo actualizar los datos del instructor");
            }
        }

    }
}
