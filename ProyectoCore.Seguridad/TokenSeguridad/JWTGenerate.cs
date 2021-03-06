using Microsoft.IdentityModel.Tokens;
using ProyectoCore.Aplicacion.Contratos.Interfaces;
using ProyectoCore.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Seguridad.TokenSeguridad
{
    public class JWTGenerate : IJWTGenerate
    {
        public string CrearToken(Usuario Usuario, List<string> Roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, Usuario.UserName)
            };
            if (Roles != null)
            {
                foreach (var Rol in Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role,Rol));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credenciales
            };

            var tokenManejador = new JwtSecurityTokenHandler();
            //El token se basa en el tokenDescripcion Para ser creado. 

            var token = tokenManejador.CreateToken(tokenDescripcion);
            
            return tokenManejador.WriteToken(token); 

        }
    }
}
