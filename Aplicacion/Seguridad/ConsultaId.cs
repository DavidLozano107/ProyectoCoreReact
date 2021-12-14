using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoCore.Aplicacion.ManejadorError;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Seguridad
{
    public class ConsultaId
    {
        public class Ejectuta: IRequest<IdentityRole>
        {
            public string Id { get; set; }
        }

        public class ValidacionEjecuta:AbstractValidator<Ejectuta>
        {
            public ValidacionEjecuta()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class EjecutaHandler : IRequestHandler<Ejectuta, IdentityRole>
        {
            private readonly CursosOnlineContext _context;

            public EjecutaHandler(CursosOnlineContext _context)
            {
                this._context = _context;
            }
            public async Task<IdentityRole> Handle(Ejectuta request, CancellationToken cancellationToken)
            {
                var Rol = await _context.Roles.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (Rol is null)
                {
                    throw new ManejadorExepcion(System.Net.HttpStatusCode.NotFound, new { Mensaje = "El rol con ese id no existe, Por favor valida nuvamente" });
                }
                return Rol;

            }
        }
    }
}
