using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProyectoCore.Aplicacion.ManejadorError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProyectoCore.API.Middleware
{
    public class ManejadorErrorMidleware
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ManejadorErrorMidleware> Logger;

        public ManejadorErrorMidleware(RequestDelegate Next, ILogger<ManejadorErrorMidleware> Logger )
        {
            this.Next = Next;
            this.Logger = Logger;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                await ManejadorExepcionAsincrona(context, ex, Logger);
            }
        }

        private async Task ManejadorExepcionAsincrona(HttpContext context, Exception ex, ILogger<ManejadorErrorMidleware> logger)
        {
            object errores = null;

            switch (ex)
            {
                case ManejadorExepcion me:
                    logger.LogError(ex, "Manejador error");
                    errores = me.Errores;
                    context.Response.StatusCode = (int)me.Codigo;
                    break;
                case Exception e:
                    logger.LogError(ex, "Error de servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                default:
                    break;
            }
            context.Response.ContentType = "application/json";

            if (errores != null)
            {
                var result = JsonConvert.SerializeObject(new { errores });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
