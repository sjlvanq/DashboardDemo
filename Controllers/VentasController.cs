using AutoMapper;
using DashboardDemo.Entities.DTOs.ErrorResponses;
using DashboardDemo.Entities.DTOs.Ventas;
using DashboardDemo.Entities.Identity.Users;
using DashboardDemo.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DashboardDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "CEO, Mozo")]
    public class VentasController : ControllerBase
    {
        private readonly DashboardDemoDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public VentasController(DashboardDemoDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "CEO")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaCreateDto>>> GetVentas()
        {
            var ventas = await _context.Ventas.ToListAsync();
            var ventasDto = _mapper.Map<List<VentaCreateDto>>(ventas);
            return Ok(ventasDto);
        }

        [HttpGet("{ventaId}")]
        public async Task<ActionResult<VentaCreateDto>> GetVenta(int ventaId)
        {
            // ValidationHelper.ValidateRouteParams innecesario
            if (ventaId <= 0) {ModelState.AddModelError("error", "Identificador de venta erróneo.");}

            var venta = await _context.Ventas.FindAsync(ventaId);
            if (venta == null) {ModelState.AddModelError("error", "El registro de venta referenciado no existe.");}

            if (ModelState.ErrorCount > 0)
            {
                return ValidationProblem(ModelState);
            }

            var ventaDto = _mapper.Map<VentaCreateDto>(venta);
            return Ok(ventaDto);
        }

        [Authorize(Roles = "Mozo")]
        [HttpPost]
        public async Task<ActionResult<VentaCreateDto>> PostVenta(VentaCreateDto ventaDto)
        {
            if (ventaDto == null) { ModelState.AddModelError("error", "La solicitud no contiene datos válidos para la venta."); }

            var mesaExistente = await _context.Mesas.AnyAsync(m => m.MesaId == ventaDto.MesaId);

            if (!mesaExistente) { ModelState.AddModelError("error", "La mesa referenciada no existe."); }

            var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var usuarioExistente = await _userManager.FindByIdAsync(usuarioId);

            if (usuarioExistente == null) { ModelState.AddModelError("error", "Usted no existe."); }

            if (ModelState.ErrorCount > 0)
            {
                return ValidationProblem(ModelState);
            }

            var venta = _mapper.Map<Venta>(ventaDto);
            venta.MozoId = usuarioId;

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();
            return Ok(new { newUpdMsg = $"Venta registrada" });
        }

        [Authorize(Roles="CEO")]
        [HttpPost("historial")]
        public async Task<IActionResult> GetHistorial(VentaFiltroDto filtro)
        {
            var ventas = _context
                .Database.SqlQueryRaw<HistorialDeVentasProcDto>
                ("EXEC dbo.ObtenerHistorialDeVentas @IntervaloInicio, @IntervaloFin",
                    new SqlParameter("IntervaloInicio", filtro.IntervaloInicio),
                    new SqlParameter("IntervaloFin", filtro.IntervaloFin));

            return Ok(ventas);
        }

        [Authorize(Roles="Mozo")]
        [HttpGet("mesas")]
        public IActionResult GetMesasMozo()
        {
            try
            {
                //Id del usuario que realiza la consulta
                var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var asignaciones = _context
                    .Database.SqlQueryRaw<MesasDeMozoProcDto>
                    ("EXEC dbo.ObtenerMesasDeMozoEnTurnoActual @MozoId",
                        new SqlParameter("MozoId", usuarioId));

                return Ok(asignaciones);
            }
            catch (Exception)
            {
                return StatusCode(500, new InternalServerErrorResponse("Error desconocido al obtener mesas del turno"));
            }
        }

        [Authorize(Roles = "CEO")]
        [HttpGet("filtrar")]
        public async Task<ActionResult<IEnumerable<VentaReadOrUpdateDto>>> GetVentasPorFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            if (fechaInicio == DateTime.MinValue || fechaFin == DateTime.MinValue){
                return BadRequest("Deben especificarse las fechas del intervalo");
            }

            if (fechaInicio > fechaFin){
                return BadRequest("Intervalo de fechas inválido");
            }

            var ventas = await _context.Ventas
                .Where(v => v.FechaHoraVenta >= fechaInicio && v.FechaHoraVenta <= fechaFin)
                .ToListAsync();

            var ventasDto = _mapper.Map<List<VentaCreateDto>>(ventas);
            return Ok(ventasDto);
        }
    }
}
