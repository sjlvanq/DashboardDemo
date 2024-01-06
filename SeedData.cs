using DashboardDemo.Entities.Identity.Roles;
using DashboardDemo.Entities.Identity.Users;
using Microsoft.AspNetCore.Identity;

namespace DashboardDemo
{
    public enum Roles
    {
        CEO,
        Gerente,
        Mozo
    }

    public static class SeedData
    {
        public static void SeedDB(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(userManager, roleManager);
            SeedSuperuser(userManager, roleManager);
        }

        private static Dictionary<Roles, IdentityResult> SeedRoles(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            Dictionary<Roles, IdentityResult> roleResult = new Dictionary<Roles, IdentityResult>();

            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                string roleName = role.ToString();

                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    int hierarchyLevel = (int)role;
                    IdentityResult result = roleManager.CreateAsync(
                            new ApplicationRole(roleName, hierarchyLevel: hierarchyLevel)
                            {
                                Id = Guid.NewGuid().ToString()
                            }
                        ).Result;
                    roleResult.Add(role, result);
                }
            }

            return roleResult;
        }

        private static void SeedSuperuser(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            if (userManager.FindByEmailAsync("ceo@localhost").Result == null)
            {
                var superuser = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "CEO",
                    FirstName = "CEO",
                    LastName = "Organización",
                    Email = "ceo@localhost",
                    EmailConfirmed = true,
                    IsActive = true
                };

                IdentityResult result = userManager.CreateAsync(
                        superuser, "1234").Result;

                if (result.Succeeded)
                {
                    string firstRoleName = Enum.GetNames(typeof(Roles)).FirstOrDefault();
                    userManager.AddToRoleAsync(superuser, firstRoleName).Wait();

                    /*
                    // Asignar todos los roles a un usuario
                    foreach (Roles role in Enum.GetValues(typeof(Roles)))
                    {
                        string roleName = role.ToString();
                        userManager.AddToRoleAsync(superUser, roleName).Wait();
                    }
                    */
                }
            }
        }
    }
}
