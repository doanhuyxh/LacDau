using LacDau.Data;
using LacDau.Models;
using LacDau.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LacDau.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        public UserAPIController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config, ApplicationDbContext context) {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _context = context;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LoginViewModel vm)
        {
            var rs = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
            if (!rs.Succeeded)
            {
                return Unauthorized();
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, vm.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtConfig:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _config["JwtConfig:Issuer"],
                audience: _config["JwtConfig:Audience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
            );

            var res = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(res);
        }

    }
}
