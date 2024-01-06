using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs
{
    public class PersonalCreateDto : PersonalBaseDto
    {
        [Required(ErrorMessage = "No se ha especificado el DNI")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "Ingresar el DNI sin puntos, 7 u 8 caracteres, sólo números.")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "No se ha especificado una contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El tipo de personal (rol) es requerido")]
        public string Role { get; set; }

    }
}
