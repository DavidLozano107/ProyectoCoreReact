using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProyectoCore.Dominio.Entidades;
using ProyectoCore.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCore.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var HostServer = CreateHostBuilder(args).Build();
            
            using (var ambinete = HostServer.Services.CreateScope())
            {
                var services = ambinete.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<Usuario>>();


                    var context = services.GetRequiredService<CursosOnlineContext>();
                    context.Database.Migrate();

                    DataPrueba.InsertarData(context, userManager).Wait();


                }
                catch (Exception e)
                {
                    var logging = services.GetRequiredService<ILogger<Program>>();
                    logging.LogError(e,e.Message,"Ocurrio un error en la migración");
                }
            }
            HostServer.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
