using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoCore.Aplicacion.Seguridad
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<IdentityRole>>{}

        public class EjecutaManejador : IRequestHandler<Ejecuta, List<IdentityRole>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            public CursosOnlineContext _Context { get; }

            public EjecutaManejador(RoleManager<IdentityRole> _roleManager, CursosOnlineContext _context)
            {
                this._roleManager = _roleManager;
                this._Context = _context;
            }


            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var Roles = await _Context.Roles.ToListAsync();
                return Roles;
            }
        }

    }
}
