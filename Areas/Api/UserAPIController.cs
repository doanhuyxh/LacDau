using LacDau.Data;
using LacDau.Models;
using LacDau.Models.AccountViewModels;
using LacDau.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
    
namespace LacDau.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public UserAPIController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config, ApplicationDbContext context, ICommon common, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _context = context;
            _icommon = common;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                var us = await _userManager.FindByNameAsync(vm.UserName);
                if (us != null)
                {
                    ApplicationUser user = vm;
                    user.Avatar = "/upload/avatar/blank_avatar.png";
                    user.FirstName = "";
                    user.LastName = "";
                    user.PhoneNumber = "";
                    user.Email = "";
                    user.IsActive = true;
                    user.Address = "";
                    user.NormalizedEmail = "";
                    user.EmailConfirmed = false;
                    await _userManager.CreateAsync(user, vm.PasswordHash);

                    json.Message = "success";
                    json.StatusCode = 201;
                    json.Object = user;

                }
                else
                {
                    json.Object = vm;
                    json.Message = "error";
                    json.StatusCode = 300;
                }

                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.Object = vm;
                json.StatusCode = 300;
                return BadRequest(json);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(LoginViewModel vm)
        {
            var rs = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
            if (!rs.Succeeded)
            {
                return Unauthorized();
            }

            ApplicationUser user = await _userManager.FindByNameAsync(vm.UserName);

            return Ok( await GenerateJwtToken(user));
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromForm] TokenRequest request)
        {
            if (ModelState.IsValid)
            {
                var jwthandler = new JwtSecurityTokenHandler();
                try
                {
                    _tokenValidationParameters.ValidateLifetime = true;// custom for testing
                    var tokenInVerification = jwthandler.ValidateToken(request.JwtToken, _tokenValidationParameters, out var validedToken);

                    if (validedToken is JwtSecurityToken jwtSecurityToken)
                    {
                        var rs = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase);
                        if (rs == false)
                        {
                            return BadRequest();
                        }
                    }
                    var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Exp).Value);
                    var expiryDate = UnixTimeStampeToDate(utcExpiryDate);

                    if (expiryDate > DateTime.Now)
                    {
                        return BadRequest(new JsonResultVM
                        {
                            Message = "Expired token",
                            StatusCode = 415,

                        });
                    }

                    var store = await _context.RefreshToken.FirstOrDefaultAsync(x => x.Token == request.TokenRefresh);

                    if (store == null)
                    {
                        return BadRequest(new JsonResultVM
                        {
                            Message = "Invalid token",
                            StatusCode = 415,

                        });
                    }

                    if (store.IsUsed)
                    {
                        return BadRequest(new JsonResultVM
                        {
                            Message = "Invalid token",
                            StatusCode = 415,

                        });
                    }

                    if (store.IsRevoked)
                    {
                        return BadRequest(new JsonResultVM
                        {
                            Message = "Invalid token",
                            StatusCode = 415,

                        });
                    }

                    var jwi = tokenInVerification.Claims.FirstOrDefault(x => x.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti).Value;
                    if (jwi != store.JwtId)
                    {
                        return BadRequest(new JsonResultVM
                        {
                            Message = "Invalid token",
                            StatusCode = 415,

                        });
                    }

                    if (store.ExpirydDate < DateTime.Now)
                    {
                        return BadRequest(new JsonResultVM
                        {
                            Message = "Expired token",
                            StatusCode = 415,

                        });
                    }

                    store.IsUsed = true;
                    _context.RefreshToken.Update(store);
                    await _context.SaveChangesAsync();

                    ApplicationUser user = await _userManager.FindByIdAsync(store.UserId);
                    return Ok(await  GenerateJwtToken(user));

                }
                catch (Exception ex)
                {
                    return BadRequest(new JsonResultVM
                    {
                        StatusCode = 500,
                        Message = ex.Message,
                        Object = request
                    });
                }

            }
            else
            {
                return BadRequest();
            }

        }
        private DateTime UnixTimeStampeToDate(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTimeVal;
        }

        private async Task<AuthJwtViewModel> GenerateJwtToken(ApplicationUser user)
        {
       
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtConfig:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                }),
                Expires = DateTime.UtcNow.AddSeconds(double.Parse(_config["JwtConfig:ExpiresTimeFresh"])),
                SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["JwtConfig:Issuer"],
                Audience = _config["JwtConfig:Audience"],
                

            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var resfreshToken = new RefreshToken
            {
                Token = _icommon.RandomString(18), //gender strign resfres token
                JwtId = token.Id,
                CreatedDate = DateTime.UtcNow,
                ExpirydDate = DateTime.UtcNow.AddMonths(1),
                IsRevoked = false,
                IsUsed = false,
                UserId = user.Id,
            };

            await _context.RefreshToken.AddAsync(resfreshToken);
            await _context.SaveChangesAsync();

            AuthJwtViewModel resultData = new AuthJwtViewModel();

            resultData.RefreshToken = resfreshToken.Token;
            resultData.Success = true;
            resultData.Token = jwtToken;

            return resultData;
        }

    }
}
