using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProyectoCore.Aplicacion.ManejadorError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Seguridad
{
    public class EliminarRol
    {
        public class Ejecuta: IRequest
        {
            public string Nombre { get; set; }
        }

        public class ValidorEjecuta:AbstractValidator<Ejecuta>
        {
            public ValidorEjecuta()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class ManejadorEjecuta : IRequestHandler<Ejecuta>
        {
            public RoleManager<IdentityRole> _roleManager { get; }

            public ManejadorEjecuta(RoleManager<IdentityRole> _roleManager)
            {
                this._roleManager = _roleManager;
            }


            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var Rol = await _roleManager.FindByNameAsync(request.Nombre);
                if (Rol is null)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.NotFound,"Error, el rol con ese nombre no eciste");
                }

                var response = await _roleManager.DeleteAsync(Rol);
                if (response.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el rol");
            
            }
        }



    }
}
