using Microsoft.AspNetCore.Identity;
using ProyectoCore.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> userManager)
        {
            if (!userManager.Users.Any())
            {
                var usuario = new Usuario() { 
                    NombreCompleto = "David Alexander Lozano Hernandez", 
                    UserName = "DavidLozano",
                    Email = "Davidlozano107@gmail.com"
                };
                
               await userManager.CreateAsync(usuario,"Password123$");

            }
        }
    }
}
