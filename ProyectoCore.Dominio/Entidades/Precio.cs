using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Dominio.Entidades
{
    public class Precio
    {
        [Key]
        public int PrecioId { get; set; }
        public decimal PrecioActual { get; set; }
        public  decimal Promocion { get; set; }
        public int CursoId { get; set; }
        public Curso Curso { get; set; }
    }
}
