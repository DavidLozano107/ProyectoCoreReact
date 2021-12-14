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
    public class RolNuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
        }


        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class EjecutaHandler : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> roleManager;

            public EjecutaHandler(RoleManager<IdentityRole> roleManager)
            {
                this.roleManager = roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var Role = await roleManager.FindByNameAsync(request.Nombre);

                if (Role is not null)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.BadRequest, new { mensaje = "Ya existe el rol"});
                }


                var result = await roleManager.CreateAsync(new IdentityRole(request.Nombre));

                if (result.Succeeded)
                {
                    return Unit.Value;
                }


                throw new Exception("No se pudo guardar el rol");
            }
        }


    }
}
