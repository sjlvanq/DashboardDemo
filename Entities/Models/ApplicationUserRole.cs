using DashboardDemo.Entities.Identity.Roles;
using DashboardDemo.Entities.Identity.Users;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace DashboardDemo.Entities.Models
{
    [Table("DDUserRoles")]
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }

    }
}
