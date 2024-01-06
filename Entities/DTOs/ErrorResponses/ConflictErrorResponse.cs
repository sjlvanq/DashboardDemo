namespace DashboardDemo.Entities.DTOs.ErrorResponses
{
    public class ConflictErrorResponse : BaseErrorResponse
    {
        public ConflictErrorResponse(string errorMessage)
            : base(
                  "https://datatracker.ietf.org/doc/html/rfc9110#status.409", 
                  "Ha ocurrido un conflicto.", 
                  409, 
                  errorMessage)
        {
        }
    }
}
