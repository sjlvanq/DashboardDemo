using AutoMapper;
using DashboardDemo.Entities.DTOs;
using DashboardDemo.Entities.DTOs.ErrorResponses;
using DashboardDemo.Entities.Identity.Roles;
using DashboardDemo.Entities.Identity.Users;
using DashboardDemo.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalController : ControllerBase
    {
        private readonly ILogger<PersonalController> _logger;
        private readonly DashboardDemoDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper mapper;
        public PersonalController(UserManager<ApplicationUser> userManager,
                            DashboardDemoDbContext dbContext,
                            IMapper mapper,
                            RoleManager<ApplicationRole> roleManager, ILogger<PersonalController> logger)
        {
            _context = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            this.mapper = mapper;
            _logger = logger;
        }

        
        [HttpGet("{roleName}")]
        public async Task<ActionResult<List<PersonalReadDto>>> GetListPersonal(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if(role==null) {
                ModelState.AddModelError("error", "El tipo o rol de personal proporcionado para obtener su listado no es válido.");
                return ValidationProblem(ModelState);
            }
            var usersWithRole = await _userManager.GetUsersInRoleAsync(roleName);
            var activeUsers = usersWithRole.Where(u => u.IsActive);
            return Ok(mapper.Map<List<PersonalReadDto>>(activeUsers));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateAsync([FromBody] PersonalCreateDto personalCreateDto)
        {
            if (personalCreateDto == null) {
                ModelState.AddModelError("error", "Falta información del personal que se intenta registrar.");
                return ValidationProblem(ModelState);
            }
            var role = await _roleManager.FindByNameAsync(personalCreateDto.Role);
            if (role == null) {
                ModelState.AddModelError("error", "El rol proporcionado no es válido.");
                return ValidationProblem(ModelState);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);
            var currentUserRoleHierarchy = currentUserRole.Any() ? (await _roleManager.FindByNameAsync(currentUserRole.First()))?.HierarchyLevel ?? 0 : 0;
            if (currentUserRoleHierarchy < role.HierarchyLevel)
            {
                var personal = mapper.Map<ApplicationUser>(personalCreateDto);
                personal.Id = Guid.NewGuid().ToString();

                var result = await _userManager.CreateAsync(personal, personalCreateDto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(personal, role.Name);
                    return Ok(new { newUpdMsg = $"Nuevo usuario registrado: {personal.Id}" });
                }
                else
                {
                    return BadRequest(new ConflictErrorResponse("El registro del personal falló"));
                }
            }
            else
            {

                return Unauthorized(new UnauthorizedErrorResponse("Permisos insuficientes para registrar personal con rol " + role.Name));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateAsync(string id, [FromBody] PersonalUpdateDto personalUpdateDto)
        {
            var evaluando = new PersonalReadDto { Id = id };
            var evaluacion = ValidationHelper.ValidateRouteParams(this, evaluando);
            if (evaluacion != null) { return evaluacion; }

            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null) {
                ModelState.AddModelError(nameof(PersonalReadDto.Id), "Usuario no encontrado");
                return ValidationProblem(ModelState);
            }
            existingUser.FirstName = (string.IsNullOrEmpty(personalUpdateDto.FirstName)) ? existingUser.UserName : personalUpdateDto.FirstName;
            existingUser.LastName = (string.IsNullOrEmpty(personalUpdateDto.LastName)) ? existingUser.LastName : personalUpdateDto.LastName;
            existingUser.UserName = (string.IsNullOrEmpty(personalUpdateDto.DNI)) ? existingUser.UserName : personalUpdateDto.DNI;

            if (personalUpdateDto.Role != null) {
                var role = await _roleManager.FindByNameAsync(personalUpdateDto.Role);
                if (role == null || role.Name == null)
                {
                    ModelState.AddModelError(nameof(PersonalUpdateDto.Role), "El rol que intentas asignar al personal no es válido");
                    return ValidationProblem(ModelState);
                }
                var actuallyInRole = await _userManager.IsInRoleAsync(existingUser, role.Name);
                //Elimina TODOS los roles del usuario
                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                if (!removeRolesResult.Succeeded)
                {
                    //string s = "";
                    //foreach (var error in removeRolesResult.Errors)
                    //{
                    //    s+=$"Error al quitar roles: {error.Code} - {error.Description}";
                    //}
                    //return BadRequest($"No fue posible la actualización del personal 1 {s}");
                    
                    ModelState.AddModelError("error", "No fue posible la actualización del personal");
                    return ValidationProblem(ModelState);
                }
                var asignRoleResult = await _userManager.AddToRoleAsync(existingUser, role.Name);
                if (!asignRoleResult.Succeeded)
                {
                    ModelState.AddModelError("error", "No fue posible la actualización del personal");
                    return ValidationProblem(ModelState);
                    //return BadRequest("No fue posible la actualización del personal");
                }
            }
            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (updateResult.Succeeded)
            {
                return Ok(new { newUpdMsg = $"Personal {id} ha sido actualizado" });
            }
            ModelState.AddModelError("error", "No fue posible la actualización del personal");
            return ValidationProblem(ModelState);
            //return BadRequest("No fue posible la actualización del personal");
        }

        [HttpDelete("{personalId}")]
        public async Task<ActionResult<string>> SoftDeleteUserAsync(string personalId)
        {
            var evaluando = new PersonalReadDto { Id = personalId };
            var evaluacion = ValidationHelper.ValidateRouteParams(this, evaluando);
            if (evaluacion != null) { return evaluacion; }

            var user = await _userManager.FindByIdAsync(personalId);
            if (user == null) {
                ModelState.AddModelError("error", "Usuario no encontrado");
                return ValidationProblem(ModelState);
            }
            
            user.IsActive = false;
            user.UserName = user.UserName + user.Id;
            await _userManager.UpdateAsync(user);

            return Ok(new { DelMsg = $"Usuario {personalId} ha sido eliminado" });
        }
    }
}