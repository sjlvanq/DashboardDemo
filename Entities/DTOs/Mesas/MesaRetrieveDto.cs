using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs.Mesas
{
    public class MesaRetrieveDto : MesaBaseDto
    {
        // Apoyo de validación en ModelBinder Helper/CustomIntModelBinder
        [Range(1, int.MaxValue, ErrorMessage = "La mesa no ha sido referenciada correctamente")]
        public int MesaId { get; set; }
        public string? Nombre { get; set; }
    }
}
