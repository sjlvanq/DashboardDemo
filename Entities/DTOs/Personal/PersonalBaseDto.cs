using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs
{
    public class PersonalBaseDto
    {
        [RegularExpression(@"^([a-zA-ZáéíóúüñÁÉÍÓÚÜÑ]{3,}\s?)*$", ErrorMessage = "Nombres inválidos. (Opcional)")]
        public string? FirstName { get; set; }
        [RegularExpression(@"^([a-zA-ZáéíóúüñÁÉÍÓÚÜÑ]{3,}\s?)*$", ErrorMessage = "Apellidos inválidos. (Opcional)")]
        public string? LastName { get; set; }
    }
}
