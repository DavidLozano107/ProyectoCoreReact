using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProyectoCore.Aplicacion.Contratos.Interfaces;
using ProyectoCore.Aplicacion.ManejadorError;
using ProyectoCore.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly SignInManager<Usuario> signInManager;
            private readonly IJWTGenerate jWTGenerate;

            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJWTGenerate jWTGenerate)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.jWTGenerate = jWTGenerate;
            }

            public class EjecutaValidacion : AbstractValidator<Ejecuta>
            {
                public EjecutaValidacion()
                {
                    RuleFor(x => x.Email).NotEmpty();
                    RuleFor(x=>x.Password).NotEmpty();
                }
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);        
                
                if (user == null)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.NotFound,"El correo no se encuentra.");
                }

                var result= await signInManager.CheckPasswordSignInAsync(user, request.Password,false);
                
                if (result.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = user.NombreCompleto,
                        Token = jWTGenerate.CrearToken(user),
                        Username = user.UserName,
                        Email = user.Email,
                        Image = null 
                    };
                }

                throw new ManejadorExepcion(System.Net.HttpStatusCode.Unauthorized,"Error de contraseña.");

            }
        }


    }
}
