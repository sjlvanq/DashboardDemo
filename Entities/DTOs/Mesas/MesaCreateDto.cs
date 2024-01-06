using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs.Mesas
{
    public class MesaCreateDto : MesaBaseDto
    {
        [Required(ErrorMessage = "Debe proporcionar un nombre a la mesa")]
        public string Nombre { get; set; }
    }
}
