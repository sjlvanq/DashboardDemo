using System.ComponentModel.DataAnnotations;

namespace DashboardDemo.Entities.DTOs
{
    public class UserCredentials
    {
        [Required(ErrorMessage= "No ha ingresado el DNI del usuario")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Debe proporcionar la contraseña")]
        public string Password { get; set; }

    }
}
