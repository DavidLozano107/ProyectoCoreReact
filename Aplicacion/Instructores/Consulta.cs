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
    public class Consulta
    {
        public class Ejecuta : IRequest<List<InstructorModel>>
        {
            
        }


        public class Manejador : IRequestHandler<Ejecuta, List<InstructorModel>>
        {
            private readonly IInstructor instructor;

            public Manejador(IInstructor instructor)
            {
                this.instructor = instructor;
            }

            public async Task<List<InstructorModel>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var ListaInstructor = await instructor.ObtenerLista();
                return ListaInstructor.ToList();
            }
        }


    }
}
