using DashboardDemo.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardDemo.Entities.Identity.Roles
{
    [Table("DDRoles")]
    public class ApplicationRole : IdentityRole<string>
    {
        public string? Description { get; set; }
        public int HierarchyLevel { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

        public ApplicationRole(string name, string? description = null, int hierarchyLevel = 0)
            : base(name)
        {
            Description = description;
            HierarchyLevel = hierarchyLevel;
            NormalizedName = name.ToUpperInvariant();
        }
    }
}
