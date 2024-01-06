using DashboardDemo.Entities.DTOs.ErrorResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PruebaController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPrueba()
        {
            return Unauthorized(new InternalServerErrorResponse("El servidor se hizo aca"));
        }
    }
}
