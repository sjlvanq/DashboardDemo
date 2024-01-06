namespace DashboardDemo.Entities.DTOs.ErrorResponses
{
    public class UnauthorizedErrorResponse : BaseErrorResponse
    {
        public UnauthorizedErrorResponse(string errorMessage)
            : base(
                  "https://datatracker.ietf.org/doc/html/rfc9110#status.401",
                  "Acceso denegado.",
                  401,
                  errorMessage)
        {
        }
    }
}
