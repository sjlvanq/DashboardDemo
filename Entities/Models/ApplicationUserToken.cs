using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DashboardDemo.Entities.Identity.Users;

namespace DashboardDemo.Entities.Models
{
    [Table("DDUserTokens")]
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
