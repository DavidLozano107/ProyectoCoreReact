using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
        Task<IList<InstructorModel>> ObtenerLista();
        Task<InstructorModel> ObtenerInstructor(Guid Id);
        Task<int> NuevoInstructor(InstructorModel instructor);
        Task<int> EditarInstructor(Guid InstructorId, string Nombre, string Apellidos, string Titulos);
        Task<int> EliminarInstructor(Guid Id);
    }
}
