using DashboardDemo.Entities.DTOs;
using DashboardDemo.Entities.Identity.Roles;
using DashboardDemo.Entities.Identity.Tokens;
using DashboardDemo.Entities.Identity.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace DashboardDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration configuration;

        public TokensController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<ApplicationRole> roleManager,
                               IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

            this.configuration = configuration;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {
            if (userCredentials == null)
            {
                return BadRequest("No se ha enviado el cuerpo de la solicitud.");
            }
            var result = await _signInManager.PasswordSignInAsync(userCredentials.UserName,
                userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var authenticationResponse = await GenerateJwtToken(userCredentials);
                
                //HierarchyLevel
                var user = await _userManager.FindByNameAsync(userCredentials.UserName);
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault();
                var customRole = await _roleManager.FindByNameAsync(roleName);
                var hierarchyLevel = customRole?.HierarchyLevel;
                authenticationResponse.HierarchyLevel = hierarchyLevel;

                return authenticationResponse;
            }
            else
            {
                return Unauthorized(new { errors = new { error = "DNI o Contraseña incorrecto"}});
            }
        }
        private async Task<AuthenticationResponse> GenerateJwtToken(UserCredentials userCredentials)
        {

            var user = await _userManager.FindByNameAsync(userCredentials.UserName);
            var clams = new List<Claim>(){
                new Claim("UserName",  userCredentials.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var claimDb = await _userManager.GetClaimsAsync(user);
            clams.AddRange(claimDb);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["MyKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddDays(1);
            

            var token = new JwtSecurityToken(
                        issuer: null,
                        audience: null,
                         claims: clams,
                      expires: expiration,
                     signingCredentials: creds
                     );

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
