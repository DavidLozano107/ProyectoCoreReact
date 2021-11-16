using ProyectoCore.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Cursos
{
    public class CursoDTO
    {
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Byte[] FotoPortada { get; set; }
        public List<InstructorDTO> Instructores { get; set; }
        public PrecioDTO Precio { get; set; }
        public List<ComentarioDTO> Comentarios { get; set; }
    }
}
