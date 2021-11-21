using FluentValidation;
using MediatR;
using ProyectoCore.Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Instructores
{
    public class Elimina
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }
        }

        public class Validar :AbstractValidator<Ejecuta>
        {
            public Validar()
            {
                RuleFor(x => x.InstructorId).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor instructor;

            public Manejador(IInstructor instructor)
            {
                this.instructor = instructor;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await instructor.EliminarInstructor(request.InstructorId);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar al instuctor");
            }
        }

    }
}
