using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs.Ventas
{
    public class VentaFiltroDto
    {
        [Required(ErrorMessage = "No se ha especificado la fecha/hora de inicio del intervalo")]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "La fecha de inicio del intervalo no es válida.")]
        [DataType(DataType.DateTime)]
        public string IntervaloInicio { get; set; }

        [Required(ErrorMessage = "No se ha especificado la fecha/hora de fin del intervalo")]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "La fecha de final del intervalo no es válida.")]
        [DataType(DataType.DateTime)]
        public string IntervaloFin { get; set; }
    }
}
