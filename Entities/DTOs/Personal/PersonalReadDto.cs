using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs
{
    public class PersonalReadDto : PersonalBaseDto
    {
        // UUID
        [RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", ErrorMessage = "El idetificador de personal debe ser un UUID válido.")]
        public string Id { get; set; }
        public string? DNI { get; set; }
    }
}
