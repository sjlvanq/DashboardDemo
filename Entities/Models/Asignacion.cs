using DashboardDemo.Entities.Identity.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardDemo.Entities.Models
{
    [Table("DDAsignaciones")]
    public class Asignacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AsignacionId { get; set; }

        [ForeignKey("TurnoId")] //AK_TurnoMesa en Fluent
        public int TurnoId { get; set; }
        public Turno Turno { get; set; }

        [ForeignKey("MesaId")] //AK_TurnoMesa en Fluent
        public int MesaId { get; set; }
        public Mesa Mesa { get; set; }

        public string? MozoId { get; set; }
        public ApplicationUser Mozo { get; set; }
    }
}
