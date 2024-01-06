using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardDemo.Entities.Models
{
    [Table("DDVentas")]
    public class Venta
    {
        public Venta() { FechaHoraVenta = DateTime.Now; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VentaId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FechaHoraVenta { get; set; }

        [Required]
        [ForeignKey("MesaId")]
        public int MesaId { get; set; }

        [Required]
        public string MozoId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Monto { get; set; }

        public Mesa Mesa { get; set; }
    }
}

