using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ProyectoCore.Aplicacion.Cursos;
using FluentValidation.AspNetCore;
using ProyectoCore.API.Middleware;
using ProyectoCore.Dominio.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProyectoCore.Aplicacion.Contratos.Interfaces;
using ProyectoCore.Seguridad.TokenSeguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using ProyectoCore.Aplicacion.Interfaces;
using ProyectoCore.Persistencia.DapperConexion;
using ProyectoCore.Persistencia.DapperConexion.Interfaces;
using ProyectoCore.Persistencia.DapperConexion.Instructor;
using ProyectoCore.Persistencia.DapperConexion.Paginacion;

namespace ProyectoCore.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CursosOnlineContext>(op => 
            {
                op.UseSqlServer(Configuration.GetConnectionString("DefaultConexion"));
             });

            //Dapper
            services.AddOptions();
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));

            services.AddMediatR(typeof(Consulta.Manejador).Assembly);

            //Añadimos la validación FluentValidation
            services.AddControllers(op => { 
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                op.Filters.Add(new AuthorizeFilter(policy));
            }).AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            //Configuracion Identity Core para trabajar dentro de webAPI. 
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            //RolManager
            identityBuilder.AddRoles<IdentityRole>();
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario,IdentityRole>>();


            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();

            //JWT
            services.AddScoped<IJWTGenerate, JWTGenerate>();
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();

            //AutoMapper
            services.AddAutoMapper(typeof(Consulta.Manejador));

            //Dapper
            services.AddTransient<IFactoryConnection, FactoryConnection>();
            services.AddScoped<IInstructor, InstructorRepositorio>();

            //Paginacion
            services.AddScoped<IPaginacion, PaginacionRepositorio>();

  


            var _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(op => op.TokenValidationParameters = new TokenValidationParameters { 
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                });


            services.TryAddSingleton<ISystemClock, SystemClock>();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Servicios mantenimiento de cursos", Version = "v1" });
                c.CustomSchemaIds(c => c.FullName); //Mediator
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseMiddleware<ManejadorErrorMidleware>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                
            }

            app.UseAuthentication(); //JWT
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cursos Online v1"));
        }
    }
}
