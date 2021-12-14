using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoCore.Aplicacion.Contratos.Interfaces;
using ProyectoCore.Aplicacion.ManejadorError;
using ProyectoCore.Dominio.Entidades;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
        }

        //Valida datos 

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            } 
        }



        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext context;
            private readonly UserManager<Usuario> userManager;
            private readonly IJWTGenerate iJWTGenerate;

            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJWTGenerate iJWTGenerate)
            {
                this.context = context;
                this.userManager = userManager;
                this.iJWTGenerate = iJWTGenerate;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var ExistEmail = await context.Users.Where(x => x.Email == request.Email).AnyAsync();
                var ExistUserName = await context.Users.Where(x => x.UserName == request.UserName).AnyAsync();
                
                if (ExistEmail)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.BadRequest,new { Mensaje = "Ya existe un usuario registrado con el email" });
                }

                if (ExistUserName)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.BadRequest,new { Mensaje = "Ya existe un usuario registrado con el username" });
                }

                var usuario = new Usuario()
                {
                    Email = request.Email,
                    NombreCompleto = request.Nombre + " " + request.Email,
                    UserName = request.UserName
                };

                var result = await userManager.CreateAsync(usuario, request.Password);

                if (result.Succeeded)
                {
                    return new UsuarioData()
                    {
                        Email = usuario.Email,
                        NombreCompleto = usuario.NombreCompleto,
                        Token = iJWTGenerate.CrearToken(usuario, null),
                        Username = usuario.UserName
                    };
                }

                List<string> Errores = new();
                foreach (var item in result.Errors)
                {
                    Errores.Add(item.Description);
                }
                 throw new ManejadorExepcion(System.Net.HttpStatusCode.BadRequest,Errores);
            }
        }

    }
}
