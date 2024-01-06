using DashboardDemo.Entities.Identity.Users;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace DashboardDemo.Entities.Models
{
    [Table("DDUserClaims")]
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }

    }
}
