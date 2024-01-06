using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using DashboardDemo.Entities.Identity.Roles;


namespace DashboardDemo.Entities.Models
{
    [Table("DDRoleClaims")]
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
