namespace DashboardDemo.Entities.DTOs
{
    public class PersonalUpdateDto : PersonalBaseDto
    {
        // Propiedades nullables en actualización para mantener su valor
        public string? Role { get; set; }
        public string? DNI { get; set; }
    }
}
