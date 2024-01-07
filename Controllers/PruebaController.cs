using DashboardDemo.Entities.DTOs.ErrorResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PruebaController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPrueba()
        {
            return Unauthorized(new InternalServerErrorResponse("El servidor se hizo aca"));
        }
    }
}
