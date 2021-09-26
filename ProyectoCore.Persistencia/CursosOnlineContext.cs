using Microsoft.EntityFrameworkCore;
using ProyectoCore.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Persistencia
{
    public class CursosOnlineContext :DbContext
    {
        public CursosOnlineContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {

        }

        //Definimos que hay claves primarias compuesta
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new {ci.CursoId,ci.InstructorId });
        }


        public DbSet<Curso> Curso { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Precio> Precio { get; set; }
        public DbSet<CursoInstructor> CursoInstructor { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
    }
}
