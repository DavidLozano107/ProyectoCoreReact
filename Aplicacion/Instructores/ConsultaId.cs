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
    public class ConsultaId
    {
        public class Ejecuta: IRequest<InstructorModel>
        {
            public Guid InstructorId { get; set; }
        }

        public class Validaror: AbstractValidator<Ejecuta>
        {
            public Validaror()
            {
                RuleFor(x => x.InstructorId).NotEmpty();    
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, InstructorModel>
        {
            private readonly IInstructor instructor;

            public Manejador(IInstructor instructor)
            {
                this.instructor = instructor;
            }

            public Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var InstructorEncontrado = instructor.ObtenerInstructor(request.InstructorId);

                if (InstructorEncontrado != null)
                {
                    return InstructorEncontrado;
                }

                throw new Exception("No se pudo encontrar el instructor");

            }
        }
    }
}
