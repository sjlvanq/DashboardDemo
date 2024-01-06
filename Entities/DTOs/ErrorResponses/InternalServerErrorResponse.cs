namespace DashboardDemo.Entities.DTOs.ErrorResponses
{
    public class InternalServerErrorResponse : BaseErrorResponse
    {
        public InternalServerErrorResponse(string errorMessage)
            : base(
                  "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error",
                  "Error interno del servidor.",
                  500,
                  errorMessage)
        {
        }
    }
}
