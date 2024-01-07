using DashboardDemo.Entities.Identity.Roles;
using DashboardDemo.Entities.Identity.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "CEO, Gerente")]
public class RolesController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RolesController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet("inferiores")]
    public async Task<IActionResult> GetRolesWithHigherHierarchy()
    {
        //if (User == null) { return BadRequest(); }
        var user = await _userManager.GetUserAsync(User);
        if (user == null) { return BadRequest(); }
        var userRoles = await _userManager.GetRolesAsync(user);
        var userRole = _roleManager.Roles.SingleOrDefault(role => userRoles.Contains(role.Name));

        if (userRole == null)
        {
            ModelState.AddModelError("", "El usuario no tiene roles asignados");
            return ValidationProblem(ModelState);
        }

        int userHierarchyLevel = userRole.HierarchyLevel;
        var rolesWithHigherHierarchy = _roleManager.Roles
            .Where(role => role.HierarchyLevel > userHierarchyLevel)
            .ToList();

        return Ok(rolesWithHigherHierarchy);
    }
}
