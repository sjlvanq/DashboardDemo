namespace DashboardDemo.Entities.DTOs.Ventas
{
    public class VentaCreateDto : VentaBaseDto
    {
        // Reservado: registro de venta con identidad a elección
        public string? MozoId { set; get; }
    }
}
