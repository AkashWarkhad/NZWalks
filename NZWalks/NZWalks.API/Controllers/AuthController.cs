using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO.Auth;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepo)
        {
            UserManager = userManager;
            tokenRepository = tokenRepo;
        }

        // -------------------------------------- Register --------------------------------------------
        // POST: https://localhost:7346/api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (registerRequestDto == null)
            {
                return BadRequest("Data should not be null");
            }

            var identityUser = new IdentityUser
            {
                Email = registerRequestDto.Username,
                UserName = registerRequestDto.Username
            };

            var identityResult = await UserManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await UserManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User is created successfully. Please Login..!");
                    }
                }
            }

            return BadRequest("Something went wrong..!");
        }

        // ------------------------------------ Login -------------------------------------------------
        // POST: https://localhost:7246/api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto) 
        {
            // search a user by its emailAddress which user already authorized in the system
            var user = await UserManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                // check the Password
                var checkPasswordResult = await UserManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    // Create Token against to login
                    var roles = await UserManager.GetRolesAsync(user);

                    if (roles != null && roles.Any()) 
                    {
                        var JwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponseDto()
                        {
                            JwtToken = JwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or Password is incorrect..!");
        }    
    }
}
