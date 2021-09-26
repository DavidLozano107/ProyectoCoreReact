using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Dominio.Entidades
{
    public class Curso
    {
        [Key]
        public int CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Byte[] FotoPortada { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<CursoInstructor> InstructorLink { get; set; }
    }
}
