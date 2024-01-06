using AutoMapper;
using DashboardDemo.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DashboardDemo.Helpers;
using DashboardDemo.Entities.DTOs.ErrorResponses;
using Microsoft.Data.SqlClient;
using DashboardDemo.Entities.DTOs.Turnos;
using System.Linq;


namespace DashboardDemo.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly DashboardDemoDbContext _context;
        private readonly IMapper _mapper;

        public TurnosController(DashboardDemoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Turnos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TurnoDto>>> GetAllTurnos()
        {
            var turnosMozos = await _context.Turnos.ToListAsync();
            return Ok(turnosMozos);
        }

        [HttpGet("mesas/{TurnoId}")]
        public IActionResult GetMesas(int turnoId)
        {
           if (turnoId <= 0){
                    return BadRequest("El tuno no ha sido referenciado correctamente.");
                }
            try
            {
            var asignaciones = _context
                .Database.SqlQueryRaw<MesasDeTurnoProcDto>
                ("EXEC dbo.ObtenerMesasDeTurno @TurnoId",
                    new SqlParameter("TurnoId", turnoId));

            return Ok(asignaciones);
            }
            catch (Exception)
            {
                return StatusCode(500, new InternalServerErrorResponse("Error desconocido al obtener mesas del turno"));
            }
        }

        [HttpGet("mesas-no-asignadas/{TurnoId}")]
        public IActionResult GetMesasNoAsignadas(int turnoId)
        {
            if (turnoId <= 0)
            {
                return BadRequest("El tuno no ha sido referenciado correctamente.");
            }
            try
            {
                var asignaciones = _context
                    .Database.SqlQueryRaw<MesasNoAsignadasProcDto>
                    ("EXEC dbo.ObtenerMesasNoAsignadas @TurnoId",
                        new SqlParameter("TurnoId", turnoId));

                return Ok(asignaciones);
            }
            catch (Exception)
            {
                return StatusCode(500, new InternalServerErrorResponse("Error desconocido al obtener mesas no asignadas del turno"));
            }
        }

        // POST: api/Turnos
        [HttpPost]
        public ActionResult<TurnoCreateDto> PostTurno([FromBody] TurnoCreateDto turnoCreateDTO)
        {
            if (turnoCreateDTO == null) {
                return BadRequest("Falta información del turno que se intenta registrar.");
            }

            var turno = _mapper.Map<Turno>(turnoCreateDTO);
            try
            {
                DateTime horaInicio = turno.HoraInicio;
                DateTime horaFin = turno.HoraFin;

                _context.Database.ExecuteSqlRaw("EXEC dbo.CrearTurnoYAsignarMesas @HoraInicio, @HoraFin",
                    new SqlParameter("HoraInicio", horaInicio),
                    new SqlParameter("HoraFin", horaFin));

                return Ok(new { NewUpdMsg = "El nuevo turno ha sido registrado" });
            }
            catch
            { //catch (Exception ex) (DbUpdateException ex)
                return StatusCode(500, new InternalServerErrorResponse("Error desconocido al intentar registrar el turno"));
            }
        }

        [HttpPut("asignar-mozo")]
        public IActionResult AsignarMozo([FromBody] TurnoAsignarMozoDto asignarMozoDto)
        {
            if (asignarMozoDto == null)
            {
                return BadRequest("Falta información del turno que se intenta registrar.");
            }
            //try
            //{
                int turnoId = asignarMozoDto.TurnoId;
                int mesaId = asignarMozoDto.MesaId;
                string mozoDni = asignarMozoDto.MozoDni;

                _context.Database.ExecuteSqlRaw("EXEC dbo.AsignarMozoAMesaEnTurno @TurnoId, @MesaId, @MozoDNI",
                    new SqlParameter("TurnoId", turnoId),
                    new SqlParameter("MesaId", mesaId),
                    new SqlParameter("MozoDNI", mozoDni));

                return Ok(new { NewUpdMsg = "El mozo ha sido asignado" });
            //}
            //catch
            //{ //catch (Exception ex) (DbUpdateException ex)
                //return StatusCode(500, new InternalServerErrorResponse("Error desconocido al intentar asignar el mozo"));
            //}
        }

        [HttpPut("asignar-mesa")]
        public async Task<IActionResult> AsignarMesa([FromBody] TurnoAsignarMesaDto asignarMesaDto)
        {
            if (asignarMesaDto == null)
            {
                return BadRequest("Falta información del turno que se intenta registrar.");
            }
            //try
            //{
                var mesaExistente = await _context.Mesas.AnyAsync(m => m.MesaId == asignarMesaDto.MesaId);

                if (!mesaExistente) { 
                    ModelState.AddModelError("error", "La mesa referenciada no existe.");
                    return ValidationProblem(ModelState);
                }

                var asignacion = _mapper.Map<Asignacion>(asignarMesaDto);
                _context.Asignaciones.Add(asignacion);
                await _context.SaveChangesAsync();
                return Ok(new { newUpdMsg = $"Venta registrada" });
            //}
            //catch
            //{ //catch (Exception ex) (DbUpdateException ex)
            //    return StatusCode(500, new InternalServerErrorResponse("Error desconocido al intentar asignar el mozo"));
            //}
        }

    }
}
