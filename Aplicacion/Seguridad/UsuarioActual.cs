using MediatR;
using Microsoft.AspNetCore.Identity;
using ProyectoCore.Aplicacion.Contratos.Interfaces;
using ProyectoCore.Aplicacion.Interfaces;
using ProyectoCore.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecuta : IRequest<UsuarioData> { }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly IJWTGenerate jWTGenerate;
            private readonly IUsuarioSesion usuarioSesion;

            public Manejador(UserManager<Usuario> userManager, IJWTGenerate jWTGenerate,IUsuarioSesion usuarioSesion) 
            {
                this.userManager = userManager;
                this.jWTGenerate = jWTGenerate;
                this.usuarioSesion = usuarioSesion;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByNameAsync(usuarioSesion.ObtenerUsuarioSesion());
                
                return new UsuarioData()
                {
                    NombreCompleto = usuario.NombreCompleto,
                    Username = usuario.UserName,
                    Email = usuario.Email,
                    Image = null,
                    Token = jWTGenerate.CrearToken(usuario)
                };
                
                
            }

        }
    }
}
