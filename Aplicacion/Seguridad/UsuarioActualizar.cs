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
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }

        public class ValidadorEjecuta:AbstractValidator<Ejecuta>
        {
            public ValidadorEjecuta()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }

        public class EjecutaManejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly CursosOnlineContext _context;
            private readonly IJWTGenerate _jWTGenerate;
            private readonly IPasswordHasher<Usuario> _passwordHasher;

            public EjecutaManejador(UserManager<Usuario> _userManager,
                CursosOnlineContext _context,
                IJWTGenerate _jWTGenerate, 
                IPasswordHasher<Usuario> _passwordHasher)
            {
                this._userManager = _userManager;
                this._context = _context;
                this._jWTGenerate = _jWTGenerate;
                this._passwordHasher = _passwordHasher;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var User = await _userManager.FindByNameAsync(request.UserName);
                if (User == null)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.NotFound, new { Mensaje = "No se encontro el mensaje" });
                }

                var result = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.UserName).AnyAsync();

                if (result)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.BadRequest, new { Mensaje = "El correo electronico ya esta en uso" });
                }

                User.NombreCompleto = request.Nombre + " " + request.Apellidos;
                User.PasswordHash = _passwordHasher.HashPassword(User, request.Password);
                User.Email = request.Email;

                var resultUpdate = await _userManager.UpdateAsync(User);

                var RolesUsuario = await _userManager.GetRolesAsync(User);

                if (resultUpdate.Succeeded)
                {
                    return new UsuarioData 
                    {
                        NombreCompleto = User.NombreCompleto,
                        Email = User.Email,
                        Username = User.UserName,
                        Image = null,
                        Token = _jWTGenerate.CrearToken(User, RolesUsuario.ToList())
                    };

                }

                throw new Exception("Error al actualizar, no se pudo actualizar el usuario");

            }
        }


    }
}
