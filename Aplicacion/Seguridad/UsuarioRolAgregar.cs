using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class UsuarioRolAgregar
    {
        public class Ejecuta : IRequest
        {
            public string UserName { get; set; }
            public string RolNombre { get; set; }
        }


        public class ValidadorEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidadorEjecuta()
            {
                RuleFor(x => x.RolNombre).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }
        public class ManejadorEjecuta : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public ManejadorEjecuta(UserManager<Usuario> _userManager,RoleManager<IdentityRole> _roleManager)
            {
                this._userManager = _userManager;
                this._roleManager = _roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var Rol = await _roleManager.FindByNameAsync(request.RolNombre);
                
                if (Rol is null)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.NotFound, new { Mensaje = "No se encontro el nombre del rol" });
                }
                
                var Usuario = await _userManager.FindByNameAsync(request.UserName);
                
                if (Usuario is null)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.NotFound, new { Mensaje = "No se encontro el usuario" });
                }
                
                var result = await _userManager.AddToRoleAsync(Usuario, request.RolNombre);
                
                if (result.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("Error al momento de eliminar");
            }
        }
    }
}
