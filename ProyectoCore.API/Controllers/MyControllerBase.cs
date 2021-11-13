using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ProyectoCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyControllerBase : ControllerBase
    {
        private IMediator mediator;
        protected IMediator Mediator => mediator ?? (mediator = HttpContext.RequestServices.GetService<IMediator>());
      
    }
}
