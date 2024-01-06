using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs.Turnos
{
    public class TurnoAsignarMozoDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "No se ha recibido un valor apropiado de referencia al turno.")]
        public int TurnoId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "No se ha recibido un valor apropiado de referencia a la mesa.")]
        public int MesaId { get; set; }
        [Required]
        //[RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", ErrorMessage = "El idetificador de personal debe ser un UUID válido.")]
        //Seguridad: el cliente no manejará el GUID del mozo sino su DNI. Debe ajustarse en otras partes del código
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "No se ha recibido un valor apropiado de referencia al mozo.")]
        public string MozoDni { get; set; }
    }
}
