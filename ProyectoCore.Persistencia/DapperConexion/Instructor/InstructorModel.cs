using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Persistencia.DapperConexion.Instructor
{
    public class InstructorModel
    {
        public Guid InstructorID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
        public DateTime? FechaCreacion { get; set; }

    }
}
