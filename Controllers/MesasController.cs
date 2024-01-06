using AutoMapper;
using DashboardDemo.Entities.DTOs.Mesas;
using DashboardDemo.Entities.Identity.Users;
using DashboardDemo.Entities.Models;
using DashboardDemo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DashboardDemo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MesasController : ControllerBase
    {
        private readonly DashboardDemoDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public MesasController(DashboardDemoDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MesaRetrieveDto>>> GetMesas()
        {
            var mesas = await _context.Mesas.ToListAsync();
            var activeMesas = mesas.Where(u => u.IsActive);
            return Ok(activeMesas);
        }

        [HttpGet("{mesaId}")]
        public async Task<ActionResult<MesaRetrieveDto>> GetMesa([FromRoute] int mesaId)
        {
            var evaluando = new MesaRetrieveDto { MesaId = mesaId };
            var evaluacion = ValidationHelper.ValidateRouteParams(this, evaluando);
            if (evaluacion != null) { return evaluacion; }

            var mesa = await _context.Mesas.FindAsync(mesaId);
            if (mesa == null){
                ModelState.AddModelError("error", "La mesa no existe");
                return ValidationProblem(ModelState);
            }
            return Ok(mesa);
        }
        
        [HttpPost]
        public async Task<ActionResult<MesaBaseDto>> PostMesa([FromBody] MesaCreateDto mesaCreateDto)
        {
            if (mesaCreateDto == null) {
                return BadRequest("Falta información de la mesa que se intenta registrar.");
            };
            var mesa = _mapper.Map<Mesa>(mesaCreateDto);
            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();
            var mesaRetrieved = _mapper.Map<MesaRetrieveDto>(mesa);
            return Ok(new { newUpdMsg = $"Mesa \"{mesaRetrieved.Nombre}\" registrada" });
        }

/*
        [HttpPost("{mesaId}/asignar-mozo")]
        public async Task<IActionResult> AsignarMozoAMesa(int mesaId, [FromBody] MesaAsignarMozoDto asignarMozoDto)
        {
            var evaluando = new MesaRetrieveDto { MesaId = mesaId };
            var evaluacion = ValidationHelper.ValidateRouteParams(this, evaluando);
            if (evaluacion != null){ return evaluacion; }

            if (asignarMozoDto == null || string.IsNullOrEmpty(asignarMozoDto.MozoId)) {
                return BadRequest("Falta información del mozo que se intenta asignar a la mesa."); }
            if (asignarMozoDto.TurnoMozoId == 0) {
                ModelState.AddModelError(nameof(MesaAsignarMozoDto.TurnoMozoId), "No se ha especificado un turno.");
                return ValidationProblem(ModelState);
            }

            var mesa = await _context.Mesas.FindAsync(mesaId);
            if (mesa == null) {
                ModelState.AddModelError(nameof(MesaRetrieveDto.MesaId), "Mesa no encontrada.");
                return ValidationProblem(ModelState);
            }

            var mozo = await _userManager.FindByIdAsync(asignarMozoDto.MozoId);
            if (mozo == null) {
                ModelState.AddModelError(nameof(MesaAsignarMozoDto.TurnoMozoId), "Mozo no encontrado.");
                return ValidationProblem(ModelState);
            }

            var turnoMozo = await _context.Turnos
                .Include(tm => tm.MozosTurnos)
                .FirstOrDefaultAsync(tm => tm.Id == asignarMozoDto.TurnoMozoId);
            if (turnoMozo == null || turnoMozo.MozosTurnos == null) {
                ModelState.AddModelError(nameof(MesaAsignarMozoDto.MozoId), "Mozo no encontrado.");
                return ValidationProblem(ModelState);
            }

            var mozoTurnoExistente = await _context.Mozos
                .FirstOrDefaultAsync(mt => mt.MozoId == asignarMozoDto.MozoId && mt.TurnoMozoId == asignarMozoDto.TurnoMozoId);
            if (mozoTurnoExistente == null) {
                ModelState.AddModelError(nameof(MesaAsignarMozoDto.MozoId), "El mozo debe estar asignado al turno antes de asignársele una mesa.");
                return ValidationProblem(ModelState);
            }

            mozoTurnoExistente.MesaId = mesaId;
            await _context.SaveChangesAsync();
            return NoContent();
        }
*/

        [HttpDelete("{mesaId}")]
        public async Task<ActionResult<string>> SoftDeleteMesaAsync(int mesaId)
        {
            var validando = new MesaRetrieveDto { MesaId = mesaId };
            var validacion = ValidationHelper.ValidateRouteParams(this, validando);
            if (validacion != null) { return validacion; }

            var mesa = await _context.Mesas.FindAsync(mesaId);
            if (mesa == null)
            {
                ModelState.AddModelError("error", "Mesa no encontrada.");
                return ValidationProblem(ModelState);
            }

            mesa.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new { DelMsg = $"La mesa \"{mesa.Nombre}\" ha sido eliminada" });
        }
    }
}
