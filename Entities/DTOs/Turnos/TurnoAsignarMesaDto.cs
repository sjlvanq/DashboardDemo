using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs.Turnos
{
    public class TurnoAsignarMesaDto
    {
        [Required]
        public int TurnoId { get; set; }
        [Required]
        public int MesaId { get; set; }
    }
}