using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs.Mesas
{
    public class MesaBaseDto
    {
        public bool AsignarANuevosTurnos { get; set; }
        public bool isActive { get; set; } = true;

    }
}
