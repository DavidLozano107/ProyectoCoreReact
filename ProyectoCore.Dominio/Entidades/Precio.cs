using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Dominio.Entidades
{
    public class Precio
    {
        [Key]
        public Guid PrecioId { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioActual { get; set; }

        [Column(TypeName ="decimal(18,5)")]
        public  decimal Promocion { get; set; }
        public Guid CursoId { get; set; }
        public Curso Curso { get; set; }
    }
}
