using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs.Turnos
{
    public class TurnoDto : TurnoBaseDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "El turno no ha sido referenciado correctamente")]
        public int TurnoId { get; set; }
    }
}
