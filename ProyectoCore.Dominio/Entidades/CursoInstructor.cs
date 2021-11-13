using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Dominio.Entidades
{
    public class CursoInstructor
    {
        public Guid CursoId { get; set; }
        public Curso Curso { get; set; }
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
    }
}
