using DashboardDemo.Entities.Identity.Roles;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace DashboardDemo.Entities.Models
{
    [Table("DDRoleClaims")]
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
