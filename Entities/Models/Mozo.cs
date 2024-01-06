using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DashboardDemo.Entities.Identity.Users;

namespace DashboardDemo.Entities.Models
{
    [Table("DDMozosTurnos")]
    public class Mozo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MozoId { get; set; }

        public ApplicationUser PersonalMozo { get; set; }

        public ICollection<Asignacion> Asignaciones { get; set; }
    }
}
