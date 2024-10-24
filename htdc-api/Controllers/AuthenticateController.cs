using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using htdc_api.Authentication;
using htdc_api.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace htdc_api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthenticateController : BaseController
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthenticateController(
        IConfiguration configuration,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<IdentityUser> userManager) : base(context, mapper, userManager)
    {
        _configuration = configuration;
        _roleManager = roleManager;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userProfile = _context.UserProfiles.FirstOrDefault(x => x.AspNetUserId == user.Id);
            if (userProfile == null || (userProfile != null && !userProfile.IsActive))
            {
                return Unauthorized();
            }

            var lastLogin = userProfile.LastLogin != null ? userProfile.LastLogin.Value : DateTime.MinValue;
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("BranchId", userProfile.BranchId.Value.ToString(), ClaimValueTypes.String)
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);

            userProfile.LastLogin = DateTime.UtcNow;
            _context.Entry(userProfile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                role = userRoles,
                username = model.Username,
                firstName = userProfile.FirstName,
                lastName = userProfile.LastName,
                lastLogin = lastLogin,
                branchId = userProfile.BranchId,
                id = user.Id
            });
        }

        return Unauthorized();
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}