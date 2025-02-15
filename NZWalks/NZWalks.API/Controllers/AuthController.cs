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
        public ILogger<AuthController> Logger { get; }

        public AuthController(UserManager<IdentityUser> userManager, 
            ITokenRepository tokenRepo,
            ILogger<AuthController> logger)
        {
            UserManager = userManager;
            tokenRepository = tokenRepo;
            Logger = logger;
        }

        // -------------------------------------- Register --------------------------------------------
        // POST: https://localhost:7346/api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto regReqDto)
        {
            if (regReqDto == null)
            {
                return BadRequest("Data should not be null");
            }

            if (IsNotValidRoles(regReqDto.Roles))
            {
                return BadRequest("Provide correct roles information from (Reader/Writer) & only 2 entires alloweded!!!");
            }

            var identityUser = new IdentityUser
            {
                Email = regReqDto.Username,
                UserName = regReqDto.Username
            };

            // Crating a new identity User
            var identityResult = await UserManager.CreateAsync(identityUser, regReqDto.Password);

            if (identityResult.Succeeded)
            {
                if (regReqDto.Roles != null && regReqDto.Roles.Any())
                {
                    identityResult = await UserManager.AddToRolesAsync(identityUser, regReqDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User is created successfully. Please Login..!");
                    }
                    else if(identityResult.Errors.Any())
                    {
                        return BadRequest($"ErrorCode = {identityResult.Errors.First().Code}," +
                            $" ErrorMessage = {identityResult.Errors.First().Description}");
                    }
                }
            }

            Logger.LogError($"User: {regReqDto.Username} registration failed!!!");
            return BadRequest($"ErrorCode = {identityResult.Errors.First().Code}," +
                            $" ErrorMessage = {identityResult.Errors.First().Description}");
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
                            JwtToken = "Bearer " + JwtToken
                        };

                        Logger.LogInformation($"{loginRequestDto.Username} Successfully logged into the sysytem");
                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or Password is incorrect..!");
        }

        private static bool IsNotValidRoles(string[] roles)
        {
            if (roles.Length == 0 || roles.Length > 2)
                return true;

            return !(roles.All(role => role.Contains("Reader") || role.Contains("Writer")));
        }
    }
}
