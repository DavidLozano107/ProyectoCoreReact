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
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Seguridad
{
    public class ObtenerRolesByUsuario
    {
        public class Ejecuta : IRequest<List<string>>
        {
            public string UserName { get; set; }
        }

        public class ValidacionEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidacionEjecuta()
            {
                RuleFor(x => x.UserName).NotEmpty();
            }
        }

        public class ManejadorEjecuta : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public ManejadorEjecuta(UserManager<Usuario> _userManager, RoleManager<IdentityRole> _roleManager)
            {
                this._userManager = _userManager;
                this._roleManager = _roleManager;
            }
            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var User = await _userManager.FindByNameAsync(request.UserName);
                if (User == null)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.NotFound, new { Mensaje = "Usuario no existe" });
                }

                var responseRoles = await _userManager.GetRolesAsync(User);
                return responseRoles.ToList();
            }
        }

    }
}
