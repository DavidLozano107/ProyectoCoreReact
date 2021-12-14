using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProyectoCore.Aplicacion.ManejadorError;
using ProyectoCore.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Seguridad
{
    public class UsuarioRolEliminar
    {
        public class Ejecutar : IRequest
        {
            public string UserName { get; set; }
            public string RolNombre { get; set; }
        }


        public class ValidatorEjecuta : AbstractValidator<Ejecutar>
        {
            public ValidatorEjecuta()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.RolNombre).NotEmpty();
            }
        }


        public class ManejadorEjecuta : IRequestHandler<Ejecutar>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public ManejadorEjecuta(UserManager<Usuario> _userManager, RoleManager<IdentityRole> _roleManager)
            {
                this._userManager = _userManager;
                this._roleManager = _roleManager;
            }

            public async Task<Unit> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var Rol = await _roleManager.FindByNameAsync(request.RolNombre);
                if (Rol == null)
                {
                    throw new ManejadorExepcion(HttpStatusCode.NotFound, new { mensaje = "El rol no existe" });
                }


                var User = await _userManager.FindByNameAsync(request.UserName);
                if (User == null)
                {
                    throw new ManejadorExepcion(HttpStatusCode.NotFound, new { mensaje = "El usuario no existe" });
                }

                var result = await _userManager.RemoveFromRoleAsync(User, request.RolNombre);
                if (result.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el rol");    
            }
        }


    }
}