using DashboardDemo.Entities.Identity.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardDemo.Entities.Models
{
    [Table("DDTurnos")]
    public class Turno
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TurnoId { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime HoraInicio { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime HoraFin { get; set; }

        public ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();

        public ICollection<ApplicationUser> Mozos { get; set; }
    }
}
