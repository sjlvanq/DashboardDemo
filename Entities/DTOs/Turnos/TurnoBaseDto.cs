using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs.Turnos
{
    public class TurnoBaseDto
    {
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}")]
        [Required(ErrorMessage = "No se ha especificado la fecha/hora de inicio del turno")]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "El valor ingresado no es correcto.")]
        [DataType(DataType.DateTime)]
        public string HoraInicio { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}")]
        [Required(ErrorMessage = "No se ha especificado la fecha/hora de finalización del turno")]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "El valor ingresado no es correcto.")]
        [DataType(DataType.DateTime)]
        public string HoraFin { get; set; }
    }
}
