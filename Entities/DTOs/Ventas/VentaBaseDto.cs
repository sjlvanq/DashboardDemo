using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs.Ventas
{
    public class VentaBaseDto
    {
        [Required(ErrorMessage = "No se ha referenciado correctamente la mesa donde la venta se ha realizado")]
        public int MesaId { get; set; }

        [Required(ErrorMessage = "No se ha especificado ningún monto de la venta.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El monto debe contener solo números. Opcionalmente, dos decimales separados por un punto")]
        [StringLength(10, ErrorMessage = "Valor excesivo para el monto.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public string Monto { get; set; }
    }
}
