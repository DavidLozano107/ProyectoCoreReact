using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Cursos
{
    public class PrecioDTO
    {
        public Guid PrecioId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioActual { get; set; }

        [Column(TypeName = "decimal(18,5)")]
        public decimal Promocion { get; set; }
        public Guid CursoId { get; set; }
    }
}
