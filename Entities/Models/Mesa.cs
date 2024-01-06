using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardDemo.Entities.Models
{
    [Table("DDMesas")]
    public class Mesa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MesaId { get; set; }

        [Required]
        public string Nombre { get; set; } = "Mesa sin nombre";

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public bool AsignarANuevosTurnos { get; set; } = false;

        public ICollection<Turno> TurnosAsignados { get; set; } = new List<Turno>();

    }
}
