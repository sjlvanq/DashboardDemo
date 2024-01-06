namespace DashboardDemo.Entities.DTOs.ErrorResponses
{
    public class BaseErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
        //public string TraceId { get; set; }

        //public BaseErrorResponse(string errorType, string title, int status, string errorMessage, string traceId)
        public BaseErrorResponse(string errorType, string title, int status, string errorMessage)
        {
            Type = errorType;
            Title = title;
            Status = status;
            Errors = new Dictionary<string, List<string>> { { "error", new List<string> { errorMessage } } };
            //TraceId = traceId;
        }
    }
}
