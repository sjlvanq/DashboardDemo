using DashboardDemo.Entities.Identity.Users;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardDemo.Entities.Models
{
    [Table("DDUserLogins")]
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
