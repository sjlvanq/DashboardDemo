namespace DashboardDemo.Entities.Identity.Tokens
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public int? HierarchyLevel { get; set; } 
    }
}
